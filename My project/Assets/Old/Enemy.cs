using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour{
    public Transform enemy;
    public int reach;
    public GameObject collided; 
    public GameObject player;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject parent; 
    public int layerMask;
    void Start(){
        agent = parent.AddComponent<UnityEngine.AI.NavMeshAgent>();
        agent.ObstacleAvoidance = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance; 
        layerMask = ~layerMask;
    }
    void FixedUpdate(){
            RaycastHit hit;
            if(Physics.Raycast(enemy.position, Vector3.forward * enemy.rotation, out hit, reach, layerMask)){
                collided = hit.collider.gameObject; 
                if(hit.collider.gameObject.tag == "Player"){
                    agent.destination = colided.transform.position;
                }
            }
        }
    }

