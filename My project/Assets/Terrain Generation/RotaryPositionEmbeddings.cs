// Embed Matrix Meanings/Nuance
using Unity;
using UnityEngine;
using System.Collections.Generic; 
public class RotaryPositionEmbeddings : MonoBehaviour 
{
    public float ropeFactor;
    public float[,] data;
    public float[,] generateEmbeddings(Vector3 transform, int dim_size, int position) 
    {
        float[,] b = new float[];
        for(int v = 0; v < 4; v++)
        {
            for(int k = 0; k < 4; k += 2)
            {
                float theta = Mathf.Pow(ropeFactor, -2 * (v * k - 1)/dim_size);
                float angle = position * theta;
                b[v, k] = Mathf.Cos(angle);
                b[v, k + 1] = Mathf.Sin(angle);
            }
        }
    }
}