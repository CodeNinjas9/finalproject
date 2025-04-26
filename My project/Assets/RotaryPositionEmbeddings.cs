using Unity;
using UnityEngine;
using System.Collections.Generic; 
public class RotaryPositionEmbeddings:MonoBehaviour 
{
    public float ropeFactor;
    public float[] data;
    public Vector3 generateEmbeddings(Vector3 transform, int dim_size, int position) 
    {
        Matrix4x4 b = new Matrix4x4();
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
        return b.MultiplyVector(transform);
    }
}