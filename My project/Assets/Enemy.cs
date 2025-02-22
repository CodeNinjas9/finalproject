public class Enemy: MonoBehaviour(){
    public Transform enemy;
    public int reach;
    public GameObject collided; 
    public int iterations;
    public int threshold;
    public int timeSteps;
    public int hits;
    public Vector3 lastPosition;
    public int lastTimestep; 
    public GameObject player;
    public NavMeshAgent agent;
    public GameObject parent; 
    void Start(){
        agent = parent.GetComponent<NavMeshAgent>();

    }
    void FixedUpdate(){
        if(lastTimestep - timeSteps > threshold)
        {
            lastPosition = null;
            agent.destination = null; 
        }
        if(lastPosition != null && hits > 20){
            hits = 0;
            agent.destination = player.position;
        }
        hits = 0; 
        for(int i = 0; i <= iterations; i++){
            RaycastHit hit;
            if(Physics.Raycast(enemy.position, enemy.rotation, reach, layerMask)){
                collided = hit.colider.gameObject; 
                if(hit.colider.gameObject.tag == "Player"){
                    hits++;
                    lastPosition = hit.colider.gameObject.transform.position
                    lastTimestep = timeSteps;
                }
            }
        }
        timeSteps++;
    }
}

