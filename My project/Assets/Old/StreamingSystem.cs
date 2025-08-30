using System.Collections.Generic;
public class StreamingSystem : MonoBehaviour
{
    public GameObject chunkObject;
    public Dictionary<Vector2Int, GameObject> loadedChunks;
    public Dictionary<Vector2Int, int> seed; 
    public GameObject player;
    public int chunkRadius; 
    public int x;
    public int y;
    public int chunkSize; 
    void Start() 
    {
        chunks = new Dictionary<Vector2Int, GameObject>();
        for(int i = 0; i <= chunkRadius; i++)
        {
            GameObject b = new GameObject(chunkObject, new Vector3(0, 0, 0), Quaternion.Identity); 
            b.SetActive(false);
            x += 1;
            y += 1;
            loadedChunks.Add(new Vector2Int(x, y), b);
        }
    }
    void FixedUpdate()
    { 
        if(x * chunkSize < (int)player.transform.position.x && y * chunkSize < (int)player.transform.position.z)
        {
            RefreshPool();
        }
        LevelOfDetail();
    }
    public async void LevelOfDetail() 
    {
        await UnityEngine.Awaitable.BackgroundThreadAsync();
        foreach(var loaded_object in loadedChunks)
        {
            float dist = Vector3.Distance(new Vector3(loaded_object.Key.x, loaded_object.Key.y, 0), player.transform.position)
            // Implement folieated rendering here.. later...
        }
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