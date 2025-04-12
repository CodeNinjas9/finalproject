
public class Emitter: MonoBehaviour{
    public int maxBounces = 9;
    public int poolSize;
    public int audioZone;
    public int layerMask = (1 << 2) | (1 << 1);
    public GameObject player;
    public float soundCalculation; 
    void Start() 
    {
        
    }
    void FixedUpdate()
    {
        Vector3 previousVector = Vector3.forward;
        for(int i = 0; i <= poolSize; i++)
        {
            ray = new Ray(transform.position, previousVector);
            RayCastHit hit;
            RayCast cast = Physics.RayCast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask);
            int bounces = 1; 
            while(bounces <= maxBounces++)
            {
                if(cast)
                {
                    length = Vector3.Distance(ray.origin, hit.point);
                    ray = new Ray(transform.position, Vector3.reflect(ray.direction, hit.normal));
                    bounces++;
                    cast = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask);
                }
                else
                {
                    break;
                }
            }
            previousVector = previousVector * Quaternion.Euler(0, 360/poolSize, 0);
        }
        soundCalculation = (soundCalculation / Vector.Distance(transform.position, player.transform.position) * bounces) ** 1/audioZone; 
    }
}