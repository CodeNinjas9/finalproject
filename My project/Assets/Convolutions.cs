using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Convolutions: MonoBehaviour{
    public Matrix4x4 mapValues;
    public Vector3[] directions;
    public int length;
    public int vectorx;
    public int vectory;
    void Start(){
        for(int v = 0; v <= vectory; v++){
            for(int i = 0; i <= vectorx; i++){
                mapValues[i, v] *= PerlinNoiseAlgorthim(i, v);
            }
        }
    }
    void Update(){
        
    }
    float PerlinNoiseAlgorthim(int x, int y){
        float x1 = (float)x;
        float x2 = (float)y;
        return Mathf.PerlinNoise(x1, x2);
    }
}
