using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class Interactions: MonoBehaviour{
    public LayerMask layerMask;
    public Transform player;
    public int reach;
    public GameObject collided; 
    public Canvas graphicalInterface;
    public GameObject lastobject;
    void Start(){
        layerMask = LayerMask.GetMask("Player", "Background");
    }
    void FixedUpdate(){
        RaycastHit hit;
        Vector3 rotatedVector = Vector3.forward * player.rotation;
        if(Physics.Raycast(player.position, rotatedVector, out hit, reach, layerMask)){
            collided = hit.collider.gameObject; 
            lastobject = collided;
            Light UIComponent = collided.AddComponent<Light>(); 
            UIComponent.color = Color.Yellow;
            UIComponent.intensity = 4;
        }
        else{
            Destroy(lastobject.GetComponent<Light>());
        }
    }
}

