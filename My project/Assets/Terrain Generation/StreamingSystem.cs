using System.Collections.Generic;
using UnityEngine;
using Unity; 
public class StreamingSystem : MonoBehaviour
{
    public Dictionary<Vector2Int, GameObject> loadedChunks;
    public Dictionary<Vector2Int, int> seed; 
    public int chunkRadius; 
    public int x;
    public int y;
    public int chunkSize; 
    public int poolSize;
    void Awake()
    {
        loadedChunks = new Dictionary<Vector2Int, GameObject>();
        seed = new Dictionary<Vector2Int, GameObject>();
    }
    void FixedUpdate()
    { 
        if(x * chunkSize < (int)player.transform.position.x && y * chunkSize < (int)player.transform.position.z)
        {
            RefreshPool();
        }
        LevelOfDetail();
    }
    public async void RefreshPool() 
    {
        await UnityEngine.Awaitable.BackgroundThreadAsync();
        foreach(var a_object in loadedChunks)
        {
            Destroy(a_object.value);
        }
        for(int i = 0; i <= poolSize; i++)
        {
            GameObject b = new GameObject(chunkObject, new Vector3(0, 0, 0), Quaternion.Identity);
            b.SetActive(false);
            x += 1;
            y += 1;
            loadedChunks.Add(new Vector2Int(x, y), b);
        }
        await UnityEngine.Awaitable.MainThreadAsync();
    }
    GameObject obtainObject(Vector2Int index)
    {
        return loadedChunks.get(index);
    }
}