using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectVisible : MonoBehaviour{
    void OnBecameVisible(){
        enabled = true;
    }
    void OnBecameInvisible(){
        enabled = false;
    }
}