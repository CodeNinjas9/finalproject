using Unity.Physics;
using Unity.UI;
public class Interactions: MonoBehaviour(){
    public LayerMask layerMask;
    public Transform player;
    public int reach;
    public GameObject collided; 
    public Canvas graphicalInterface;
    void Start(){
        layerMask = LayerMask.GetMask("Player", "Background");
    }
    void FixedUpdate(){
        RaycastHit hit;
        if(Physics.Raycast(player.position, player.rotation, reach, layerMask)){
            collided = hit.colider.gameObject; 
            Light UIComponent = collided.AddComponent<Light>(); 
            UIComponent.color = Color.Yellow;
            UIComponent.intensity = 4;
        }
    }
}

