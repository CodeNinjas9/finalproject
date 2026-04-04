using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
public class PlayerController : MonoBehaviour
{
    
    struct MovementJob : IJob
    {
        public GameObject player;
        public int movementType;
        public void Execute()
        {
            switch(movementType)
            {
                case 1:
                    player.transform.Translate(Vector3.up, Space.World);
                    break;
                case 2:
                    player.transform.Translate(Vector3.down, Space.World);
                    break;
                case 3:
                    player.transform.Translate(Vector3.left, Space.World);
                    break;
                case 4:
                    player.transform.Translate(Vector3.right, Space.World);
                    break;
            }
        }
        struct BreakJob: IJob
        {
            public GameObject k;
            public void Execute()
            {
                Destroy(k);
            }
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
