<<<<<<< HEAD
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
=======
public class RotaryPositionEmbeddings : MonoBehaviour 
{
    public float ropeFactor;
    public float[] data;
    public float[] getRotations(int dim_size, int position) 
    {
        float[] b = new float[array.Length];
        float sum = 0;
        foreach(float item in data)
        {
            sum += item;
        }
        for(int i = 0; i <= data.Length / 2; i++)
        {
            float theta = ropeFactor ** (-2(i - 1)/dim_size);
            float angle = position * theta;
            b[i] = Math.Cos(angle);
            b[i + 1] = Math.Sin(angle);
        }
        return b;
>>>>>>> origin
    }
}