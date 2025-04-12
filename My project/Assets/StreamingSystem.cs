using System.Collections.Generic;
public class StreamingSystem : MonoBehaviour
{
    public int pooliteration; 
    public int poolSize; 
    public int refreshTime; 
    [SerializeField]
    public GameObject storedObject; 
    [SerializeField]
    public List<GameObject> pool;
    
    void Start() 
    {
        pooledObjects = new List<GameObject>;
        for(int i = 0; i <= poolSize; i++)
        {
            GameObject b = new GameObject(storedObject, new Vector3(0, 0, 0), Quaternion.Identity);
            b.SetActive(false);
            pool.Add(b);
        }
    }
    void RefreshPool() 
    {
        foreach(GameObject a_object in pool)
        {
            Destroy(a_object);
        }
        for(int i = 0; i <= poolSize; i++)
        {
            GameObject b = new GameObject(storedObject, new Vector3(0, 0, 0), Quaternion.Identity);
            b.SetActive(false);
            pool.Add(b);
        }
    }
    IEnumerator RefreshPoolTask()
    {
        yield return new WaitForSeconds(refreshTime);
        RefreshPool();
        StartCouroutine("RefreshPoolTask");
    }
    GameObject obtainObject(int index)
    {
        return pool.get(index);
    }
}