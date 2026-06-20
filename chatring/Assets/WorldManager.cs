using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform playerTransform;
    public int viewDistance = 4; // Number of chunks in each direction

    // Tracks active chunks by their coordinate IDs
    private Dictionary<Vector2Int, VoxelChunk> activeChunks = new Dictionary<Vector2Int, VoxelChunk>();

    void Update()
    {
        // 1. Calculate current player chunk coordinate
        int playerChunkX = Mathf.FloorToInt(playerTransform.position.x / 32f);
        int playerChunkZ = Mathf.FloorToInt(playerTransform.position.z / 32f);

        List<Vector2Int> coordinatesToKeep = new List<Vector2Int>();

        // 2. Loop through the view distance grid around the player
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkX + x, playerChunkZ + z);
                coordinatesToKeep.Add(chunkCoord);

                // If chunk doesn't exist yet, spawn it
                if (!activeChunks.ContainsKey(chunkCoord))
                {
                    Vector3 worldPos = new Vector3(chunkCoord.x * 32, 0, chunkCoord.y * 32);
                    GameObject chunkObj = Instantiate(chunkPrefab, worldPos, Quaternion.identity);
                    VoxelChunk newChunk = chunkObj.AddComponent<VoxelChunk>();
                    
                    activeChunks.Add(chunkCoord, newChunk);
                }
            }
        }

        // 3. Clean up old chunks that are out of view distance
        List<Vector2Int> toDestroy = new List<Vector2Int>();
        foreach (var coord in activeChunks.Keys)
        {
            if (!coordinatesToKeep.Contains(coord))
                toDestroy.Add(coord);
        }

        foreach (var coord in toDestroy)
        {
            Destroy(activeChunks[coord].gameObject);
            activeChunks.Remove(coord);
        }
    }
}