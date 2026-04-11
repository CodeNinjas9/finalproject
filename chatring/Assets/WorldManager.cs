using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Transform player;
    private int renderDistance = 16; 
    private Dictionary<Vector3Int, GameObject> chunkList;

    void Awake()
    {
        chunkList = new();
    }
    void GenerateChunk(int offsetX, int offsetY, int offsetZ)
    {
        GameObject k = new GameObject("Chunk" + chunkList.Count().ToString());
        k.AddComponent<Transform>();
        k.transform.position = new Vector3(Mathf.Floor(player.transform.position.x + offsetX * 16), Mathf.Floor(player.transform.position.y + offsetY * 16), Mathf.Floor(player.transform.position.z + offsetZ * 16));
        chunkList.Add(new Vector3Int((int)player.transform.position.x + offsetX, (int)player.transform.position.y + offsetY, (int)player.transform.position.z + offsetZ), k);
        k.AddComponent<VoxelChunk>();
    }
    IEnumerator<WaitForSeconds> GenerateChunks()
    {
        yield return new WaitForSeconds(2f);
        GenerateChunk(1, 0, 0);
        GenerateChunk(-1, 0, 0);
        GenerateChunk(0, 0, 0);
        GenerateChunk(0, 0, -1);
        GenerateChunk(0, 0, 1);
    }
    void Update()
    {
        StartCoroutine(GenerateChunks());
    }
}