
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using Unity.VisualScripting;
using System.Linq;
using Unity.Mathematics;
using PerlinN;

[System.Serializable]
public struct Voxel
{
    public byte blockId;    
    public byte light;     
    public byte rotation;  
    public byte flags;     

    public bool IsAir      => blockId == 0;
    public bool IsSolid    => (flags & 1) != 0;
    public bool IsTransparent => (flags & 2) != 0;
    public bool IsLiquid   => (flags & 4) != 0;
    public bool HasCustomMesh => (flags & 8) != 0;
}


[System.Serializable]
public class BlockDefinition
{
    public string name;
    public byte flags;        
    public Vector2[] faceUVs;   
    public Mesh customMesh;     
    public Vector3 meshOffset;  
}


public static class BlockRegistry
{
    private static BlockDefinition[] blocks;

    public static void Init()
    {
        blocks = new BlockDefinition[]
        {
            new BlockDefinition {
                name  = "air",
                flags = 0b00000000,
            },
            new BlockDefinition {
                name  = "grass",
                flags = 0b00000001, 
                faceUVs = new Vector2[]
                {
                    new Vector2(0, 0), 
                    new Vector2(2, 0), 
                    new Vector2(1, 0), 
                    new Vector2(1, 0),  
                    new Vector2(1, 0),  
                    new Vector2(1, 0),  
                }
            },
            new BlockDefinition {
                name  = "dirt",
                flags = 0b00000001,
                faceUVs = new Vector2[]
                {
                    new Vector2(2, 0), new Vector2(2, 0),
                    new Vector2(2, 0), new Vector2(2, 0),
                    new Vector2(2, 0), new Vector2(2, 0),
                }
            },
            new BlockDefinition {
                name  = "stone",
                flags = 0b00000001,
                faceUVs = new Vector2[]
                {
                    new Vector2(3, 0), new Vector2(3, 0),
                    new Vector2(3, 0), new Vector2(3, 0),
                    new Vector2(3, 0), new Vector2(3, 0),
                }
            },
            new BlockDefinition {
                name  = "glass",
                flags = 0b00000011, 
                faceUVs = new Vector2[]
                {
                    new Vector2(4, 0), new Vector2(4, 0),
                    new Vector2(4, 0), new Vector2(4, 0),
                    new Vector2(4, 0), new Vector2(4, 0),
                }
            },
            new BlockDefinition {
                name  = "flower",
                flags = 0b00001000, 
                meshOffset = new Vector3(0.5f, 0f, 0.5f),
            },
        };
    }

    public static BlockDefinition Get(byte id)
    {
        if (id >= blocks.Length) return blocks[0];
        return blocks[id];
    }
}

public class VoxelChunk : MonoBehaviour
{
    public const uint chunkId = 0; 
    public const int ChunkSize = 32;
    public const float TileSize = 0.0625f; 

    private Voxel[,,] voxels = new Voxel[ChunkSize, ChunkSize, ChunkSize];

    private MeshFilter   meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;


    private List<Vector3> opaqueVerts  = new List<Vector3>();
    private List<int>     opaqueTris   = new List<int>();
    private List<Vector2> opaqueUVs    = new List<Vector2>();
    private List<Color>   opaqueColors = new List<Color>();

    private List<Vector3> transVerts  = new List<Vector3>();
    private List<int>     transTris   = new List<int>();
    private List<Vector2> transUVs    = new List<Vector2>();
    private List<Color>   transColors = new List<Color>();

    private List<GameObject> customMeshObjects = new List<GameObject>();
    
    private static readonly Vector3Int[] faceDirections = new Vector3Int[]
    {
        Vector3Int.up, Vector3Int.down,
        Vector3Int.left, Vector3Int.right,
        Vector3Int.forward, Vector3Int.back
    };

    private static readonly Vector3[][] faceVertices = new Vector3[][]
    {
        new Vector3[] {
            new Vector3(0,1,0), new Vector3(1,1,0),
            new Vector3(1,1,1), new Vector3(0,1,1)
        },
        new Vector3[] {
            new Vector3(0,0,1), new Vector3(1,0,1),
            new Vector3(1,0,0), new Vector3(0,0,0)
        },
        new Vector3[] {
            new Vector3(0,0,1), new Vector3(0,0,0),
            new Vector3(0,1,0), new Vector3(0,1,1)
        },
        new Vector3[] {
            new Vector3(1,0,0), new Vector3(1,0,1),
            new Vector3(1,1,1), new Vector3(1,1,0)
        },
        new Vector3[] {
            new Vector3(1,0,1), new Vector3(0,0,1),
            new Vector3(0,1,1), new Vector3(1,1,1)
        },
        new Vector3[] {
            new Vector3(0,0,0), new Vector3(1,0,0),
            new Vector3(1,1,0), new Vector3(0,1,0)
        }
    };
    void Awake()
    {
        BlockRegistry.Init();
        meshFilter   = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.materials = new Material[]
        {
            new Material(Shader.Find("Standard")),
            new Material(Shader.Find("Standard")) { renderQueue = 3000 }
        };
    }
    void Start()
    {
        GenerateTerrain();
        RebuildMesh();
    }
    void DestroyBlock(int x, int y, int z)
    {
        this.SetBlock(x, y, z, 0);
        RebuildMesh();
    }
    public Voxel GetVoxel(int x, int y, int z)
    {
        if (x < 0 || x >= ChunkSize ||
            y < 0 || y >= ChunkSize ||
            z < 0 || z >= ChunkSize)
            return default; // air
        return voxels[x, y, z];
    }
    public void SetVoxel(int x, int y, int z, Voxel v)
    {
        if (x < 0 || x >= ChunkSize ||
            y < 0 || y >= ChunkSize ||
            z < 0 || z >= ChunkSize)
            return;
        voxels[x, y, z] = v;
        RebuildMesh();
    }
    public void SetBlock(int x, int y, int z, byte blockId)
    {
        BlockDefinition def = BlockRegistry.Get(blockId);
        SetVoxel(x, y, z, new Voxel
        {
            blockId  = blockId,
            flags    = def.flags,
            light    = 15,
            rotation = 0
        });
    }
    public struct GenerateTerrainJob : IJob
{
    [WriteOnly] public NativeArray<Voxel> voxels;
    public float transformPosX;
    public float transformPosZ;

    public void Execute()
    {
        for (int x = 0; x < 32; x++)
        {
            for (int z = 0; z < 32; z++)
            {
                float2 noiseInput = new float2((transformPosX + x) * 0.1f, (transformPosZ + z) * 0.1f);
                float noise = Unity.Mathematics.noise.cnoise(noiseInput); 
                noise = (noise + 1f) * 0.5f; 
                int height = Mathf.FloorToInt(noise * 32);
                float normalizedNoise = (noise + 1f) * 0.5f;
                height = Mathf.Clamp(Mathf.FloorToInt(normalizedNoise * 32), 4, 31);
                for (int y = 0; y < 32; y++)
                {
                    byte id;
                    byte flags;

                    if (y < height - 3) { id = 3; flags = 0b00000001; } // Stone
                    else if (y < height)  { id = 2; flags = 0b00000001; } // Dirt
                    else if (y == height) { id = 1; flags = 0b00000001; } // Grass
                    else                  { id = 0; flags = 0b00000000; } // Air
                    int index = x + (y * 32) + (z * 32 * 32);
                    voxels[index] = new Voxel
                    {
                        blockId = id,
                        flags = flags,
                        light = 15,
                        rotation = 0
                    };
                }
            }
        }
    }
}
    public float getLayeredNoise(float x, float y)
    {
        float noise = 0;
        float frequency = 1;
        float factor = 1;
        int octaves = 6;
        for(int i = 0; i < octaves - 1; i++)
        {
            noise += Mathf.PerlinNoise(x * octaves/frequency - i * 0.49848484f * 0.292919192f, y * octaves - i * 0.72354f * 0.192822828f) * factor;  
            factor *= 0.5f;
            frequency *= 2;
        }
        return noise / 16f;
    }
    public struct ChunkData
    {
        public uint temperature;
        public uint heightMap;
        public uint moisture;

    }
    public void GenerateTerrain()
    {
        for (int x = 0; x < ChunkSize; x++)
        for (int z = 0; z < ChunkSize; z++)
        {
            float noise = getLayeredNoise(
                (transform.position.x + x) * 0.1f,
                (transform.position.z + z) * 0.1f
            );
            float biome = getLayeredNoise(
                (transform.position.x + x) * 0.1f,
                (transform.position.z + z) * 0.1f
            );
            float biomeType = 1;
            if(biome < 0.6)
            {
                biomeType *= 0.5f;
            }
            else if(biome > 0.6 && biome < 0.8)
            {
                biomeType *= 1.2f;
            }
            else
            {
                biomeType *= 2f;
            }
            int height = Mathf.FloorToInt(noise * ChunkSize * biomeType);
            for (int y = 0; y < ChunkSize; y++)
            {
                byte id;
                if      (y < height - 3) id = 3; 
                else if (y < height)     id = 2; 
                else if (y == height)    id = 1; 
                else                     id = 0; 

                BlockDefinition def = BlockRegistry.Get(id);
                voxels[x, y, z] = new Voxel
                {
                    blockId  = id,
                    flags    = def.flags,
                    light    = 15,
                    rotation = 0
                };

                
                if (id == 0 && y == height + 1)
                {
                    if (UnityEngine.Random.value < 0.05f)
                    {
                        BlockDefinition flower = BlockRegistry.Get(5);
                        voxels[x, y, z] = new Voxel
                        {
                            blockId  = 5,
                            flags    = flower.flags,
                            light    = 15,
                            rotation = 0
                        };
                    }
                }
            }
        }
    }
    void GenerateTerrainInThread()
{
    // Allocate space for the chunk grid layout (32^3)
    NativeArray<Voxel> result = new NativeArray<Voxel>(32 * 32 * 32, Allocator.TempJob);
    
    GenerateTerrainJob terrainJob = new GenerateTerrainJob
    {
        voxels = result,
        transformPosX = this.transform.position.x,
        transformPosZ = this.transform.position.z
    };

    // Schedule and instantly wait for completion
    JobHandle handle = terrainJob.Schedule();
    handle.Complete();

    // Safely copy the results back out to our managed array
    int size = VoxelChunk.ChunkSize;
    for (int x = 0; x < size; x++)
    {
        for (int y = 0; y < size; y++)
        {
            for (int z = 0; z < size; z++)
            {
                // Ensure loop order math matches perfectly with your Job's index mapping!
                int index = x + (y * size) + (z * size * size);
                this.voxels[x, y, z] = result[index];
            }
        }
    }

    result.Dispose();
}


    public void RebuildMesh()
    {
        opaqueVerts.Clear();  opaqueTris.Clear();
        opaqueUVs.Clear();    opaqueColors.Clear();
        transVerts.Clear();   transTris.Clear();
        transUVs.Clear();     transColors.Clear();

        foreach (var go in customMeshObjects)
            Destroy(go);
        customMeshObjects.Clear();

        for (int x = 0; x < ChunkSize; x++)
        for (int y = 0; y < ChunkSize; y++)
        for (int z = 0; z < ChunkSize; z++)
        {
            Voxel v = voxels[x, y, z];
            if (v.IsAir) continue;

            BlockDefinition def = BlockRegistry.Get(v.blockId);

            if (v.HasCustomMesh)
            {
                SpawnCustomMesh(def, x, y, z, v);
                continue;
            }

            AddVoxelFaces(v, def, x, y, z);
        }

        BuildMesh();
    }

    void AddVoxelFaces(Voxel v, BlockDefinition def, int x, int y, int z)
    {
        bool isTransparent = v.IsTransparent;

        for (int f = 0; f < 6; f++)
        {
            Vector3Int dir = faceDirections[f];
            Voxel neighbor = GetVoxel(x + dir.x, y + dir.y, z + dir.z);

            if (neighbor.IsSolid && !neighbor.IsTransparent) continue;
            if (isTransparent && neighbor.blockId == v.blockId) continue;

            var verts = isTransparent ? transVerts : opaqueVerts;
            var tris  = isTransparent ? transTris  : opaqueTris;
            var uvs   = isTransparent ? transUVs   : opaqueUVs;
            var cols  = isTransparent ? transColors : opaqueColors;

            int vertBase = verts.Count;
            Vector3 offset = new Vector3(x, y, z);

            foreach (var fv in faceVertices[f])
                verts.Add(RotateVertex(fv, v.rotation) + offset);

            tris.Add(vertBase + 0); tris.Add(vertBase + 2); tris.Add(vertBase + 1);
            tris.Add(vertBase + 0); tris.Add(vertBase + 3); tris.Add(vertBase + 2);

            Vector2 tileUV = def.faceUVs != null && f < def.faceUVs.Length
                ? def.faceUVs[f]
                : Vector2.zero;

            float u = tileUV.x * TileSize;
            float vv = tileUV.y * TileSize;

            uvs.Add(new Vector2(u,            vv));
            uvs.Add(new Vector2(u + TileSize, vv));
            uvs.Add(new Vector2(u + TileSize, vv + TileSize));
            uvs.Add(new Vector2(u,            vv + TileSize));

            float lightVal = v.light / 15f;
            Color col = new Color(lightVal, lightVal, lightVal, 1f);
            cols.Add(col); cols.Add(col); cols.Add(col); cols.Add(col);
        }
    }

    Vector3 RotateVertex(Vector3 v, byte rotation)
    {
        if (rotation == 0) return v;
        Vector3 centered = v - new Vector3(0.5f, 0.5f, 0.5f);
        float angle = rotation * 90f;
        Quaternion q = Quaternion.Euler(0, angle, 0);
        return q * centered + new Vector3(0.5f, 0.5f, 0.5f);
    }

    void SpawnCustomMesh(BlockDefinition def, int x, int y, int z, Voxel v)
    {
        if (def.customMesh == null) return;

        GameObject go = new GameObject($"CustomMesh_{x}_{y}_{z}");
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(x, y, z) + def.meshOffset;
        go.transform.localRotation = Quaternion.Euler(0, v.rotation * 90f, 0);

        MeshFilter mf   = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mf.mesh = def.customMesh;
        mr.material = new Material(Shader.Find("Standard"));
        customMeshObjects.Add(go);
    }

    void BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.subMeshCount = 2;

        var allVerts  = new List<Vector3>(opaqueVerts);
        allVerts.AddRange(transVerts);
        var allUVs    = new List<Vector2>(opaqueUVs);
        allUVs.AddRange(transUVs);
        var allColors = new List<Color>(opaqueColors);
        allColors.AddRange(transColors);

        var offsetTransTris = new List<int>();
        int offset = opaqueVerts.Count;
        foreach (int t in transTris)
            offsetTransTris.Add(t + offset);
        mesh.SetVertices(allVerts);
        mesh.SetUVs(0, allUVs);
        mesh.SetColors(allColors);
        mesh.SetTriangles(opaqueTris, 0);
        mesh.SetTriangles(offsetTransTris, 1);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh     = mesh;
        meshCollider.sharedMesh = mesh;
    }
    public int trailingZerosCount(uint x)
    {
        int ctn = 0;
        if (x == 0)
        {
            return 32;
        }
        uint y = x & ~x;
        while((y <<= 1) > 0)
        {
            ctn++;
        }
        return ctn;
    }
    
            
}