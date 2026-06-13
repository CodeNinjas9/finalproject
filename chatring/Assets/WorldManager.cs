using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private int initialSize = 10;
    public Material textureAtlas;
    public Transform player;
    private int renderDistance = 16; 
    private Queue<GameObject> chunkList;

    void Awake()
    {
        chunkList = new();
    }
    void GenerateChunk(int offsetX, int offsetY, int offsetZ)
    {
        print(chunkList.Count());
        while(chunkList.Count() > initialSize)
        {
            GameObject a = chunkList.Dequeue();
            Destroy(a);
        }
        GameObject k = new GameObject(chunkList.Count().ToString());
        k.AddComponent<Transform>();
        k.transform.position = new Vector3(Mathf.Floor(player.transform.position.x + offsetX * 16), Mathf.Floor(player.transform.position.y + offsetY * 16), Mathf.Floor(player.transform.position.z + offsetZ * 16));
        k.AddComponent<VoxelChunk>();
        k.AddComponent<VoxelGrid>();
        chunkList.Append(k);
    }
    void Start()
    {
        GenerateChunk(0, 0, 0);
    }
}