using UnityEngine;
using PerlinN;
using PermTables;
using Unity.Collections;
using Unity.Jobs;
public enum VoxelType { Air, Dirt, Stone, Water, Grass }



public class Chunk
{
    public VoxelType[,,] voxels = new VoxelType[16, 16, 16]; 
    public int chunkX, chunkY, chunkZ; 
    public float[,] heightmap;
}
public class ChunkGeneration : MonoBehaviour
{
    private int seed; 
    struct InstantiateChunk : IJobFor
    {
        public MeshFilter[] initialObjects;
        public Mesh[] meshes;
        public ChunkGeneration parent;
        public void Execute(int i)
        {
            CombineInstance[] instances = new CombineInstance[16 * 16 * 16];
            int index = 0; 
            Chunk chunk = parent.GenerateChunk(0 + i, 0 + i, 0 + i, Time.time);
            for(int x = 0; x < 16; x++)
            {
                for(int y = 0; y < 16; y++)
                {
                    for(int z = 0; z < 16; z++)
                    {
                        index++;
                        if(chunk.voxels[x, y, z] != VoxelType.Air)
                        {
                            MeshFilter k = initialObjects[parent.GetVoxelInt(chunk.voxels[x, y, z])];
                            k.transform.position = new Vector3(x + chunk.heightmap[x, z], y, z + chunk.heightmap[x, z]);
                            instances[index] = new CombineInstance
                            {
                                mesh = k.sharedMesh,
                                transform = k.transform.localToWorldMatrix
                            };
                        }
                    }
                }
            }
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(instances);
            meshes[i] = combinedMesh;
        }
        
    }
    
    void Awake()
    {
        Random.InitState(seed); 
    }
    void Start()
    {
        
    }
    private float[] ComputeInvFreq(int dim, float theta = 10000f)
    {
        float[] invFreq = new float[dim / 2];
        for (int i = 0; i < invFreq.Length; i++)
            invFreq[i] = 1f / Mathf.Pow(theta, (2f * i) / dim);
        return invFreq;
    }
    private Matrix4x4 GenerateRoPEForChunk(int chunkX, int chunkY, int chunkZ, float time, float[] invFreq, int dim)
    {
        Matrix4x4 ropeMatrix = Matrix4x4.identity;
        int pos = chunkX + chunkY * 1024 + chunkZ * 1024 * 1024; 
        pos += (int)(time * 100f); 
        for (int i = 0; i < dim / 16 - 1; i++)
        {
            float cosVal = Mathf.Cos(pos * invFreq[i]);
            float sinVal = Mathf.Sin(pos * invFreq[i]);
            ropeMatrix[i, i] = cosVal;
            ropeMatrix[i, i + 1] = -sinVal;
            ropeMatrix[i + 1, i + 0] = sinVal;
            ropeMatrix[i + 1, i + 1] = cosVal;
        }
        return ropeMatrix;
    }
    private float[,,] GeneratePerlinNoiseForChunk(int chunkX, int chunkY, int chunkZ, float scale, float time)
    {
        float[,,] noise = new float[16, 16, 16];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    float worldX = (chunkX * 16 + x) * scale;
                    float worldY = (chunkY * 16 + y) * scale;
                    float worldZ = (chunkZ * 16 + z) * scale;
                    float noiseVal = fractal_brownian_motion(worldX + Random.Range(0.1f, 1.5f), worldY - Random.Range(0.1f, 1.5f), Random.Range(0.1f, 1.5f)) * fractal_brownian_motion(worldY - Random.Range(0.1f, 1.5f), worldZ + Random.Range(0.5f, 2f), Random.Range(0.1f, 1.5f));
                    noiseVal += Mathf.PerlinNoise(worldX + time, worldZ + time) * 0.5f;
                    noise[x, y, z] = noiseVal;
                }
            }
        }
        return noise;
    }
    private float[,,] ApplyRoPEToNoise(float[,,] noise, Matrix4x4 ropeMatrix)
    {
        float[,,] modulatedNoise = new float[16, 16, 16];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    Vector4 noiseVec = new Vector4(noise[x, y, z], 0, 0, 0);
                    Vector4 modulatedVec = ropeMatrix * noiseVec;
                    modulatedNoise[x, y, z] = modulatedVec.x; 
                }
            }
        }
        return modulatedNoise;
    }
    private void GenerateVoxelsForChunk(Chunk chunk, float[,,] modulatedNoise)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    float noiseVal = modulatedNoise[x, y, z];
                    if (noiseVal < 0.3f)
                        chunk.voxels[x, y, z] = VoxelType.Air;
                    else if (noiseVal < 0.5f)
                        chunk.voxels[x, y, z] = VoxelType.Dirt;
                    else if (noiseVal < 0.7f)
                        chunk.voxels[x, y, z] = VoxelType.Stone;
                    else
                        chunk.voxels[x, y, z] = VoxelType.Grass;
                }
            }
        }
    }
    float fractal_brownian_motion(float x, float y, float persistence, int numOctaves = 5)
    {
        float result = 0.0f;
        float amplitude = 0.5f;
        float frequency = 1.0f;
        for (int i = 0; i < numOctaves; i++)
        {
            result += amplitude * Mathf.PerlinNoise(frequency * x, frequency * y);
            amplitude *= 0.5f;   
            frequency *= persistence;   
        }
        return result;
    }
    float rigged_noise(float x, float y, float persistence, int numOctaves = 5)
    {
        float result = 0.0f;
        float amplitude = 0.5f;
        float frequency = 1.0f;
        for (int i = 0; i < numOctaves; i++)
        {
            result += ((1.0f - Mathf.Abs(Mathf.PerlinNoise(x * frequency, y * frequency))) * 2.0f - 1.0f) * amplitude;
            amplitude *= 0.5f;   
            frequency *= persistence;   
        }
        return result;
    }
    public Chunk GenerateChunk(int chunkX, int chunkY, int chunkZ, float time)
    {
        Chunk chunk = new Chunk();
        chunk.chunkX = chunkX;
        chunk.chunkY = chunkY;
        chunk.chunkZ = chunkZ;
        float[] invFreq = ComputeInvFreq(64);
        Matrix4x4 ropeMatrix = GenerateRoPEForChunk(chunkX, chunkY, chunkZ, time, invFreq, 64);
        float[,,] noise = GeneratePerlinNoiseForChunk(chunkX, chunkY, chunkZ, 0.1f, time);
        float[,,] modulatedNoise = ApplyRoPEToNoise(noise, ropeMatrix);
        GenerateVoxelsForChunk(chunk, modulatedNoise);
        chunk.heightmap = GenerateLayeredTerrain(100, 200, Random.Range(0.1f, 2f), Random.Range(0.1f, 1.5f));
        return chunk;
    }
    private float[,] GenerateLayeredTerrain(int width, int depth, float baseScale, float heightScale)
    {
        float[,] heightmap = new float[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float baseNoise = Mathf.PerlinNoise(x * baseScale, z * baseScale);
                float detailNoise = Mathf.PerlinNoise(x * baseScale * 2f, z * baseScale * 2f) * 0.5f;
                heightmap[x, z] = (baseNoise + detailNoise) * heightScale;
            }
        }
        return heightmap;
    }
    public int GetVoxelInt(VoxelType vox)
    {
        switch(vox){
            case VoxelType.Dirt:
                return 0;
            case VoxelType.Stone:
                return 1;
            case VoxelType.Water:
                return 2;
            case VoxelType.Grass:
                return 3;
            default: 
                return -1;
        }
    }
}