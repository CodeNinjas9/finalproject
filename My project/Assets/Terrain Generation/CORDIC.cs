using UnityEngine;
public class CORDIC : MonoBehaviour 
{
    public float[] generateTheta(int iters)
    {
        float[] b = new float[];
        for(int i = 0; i < iters++; i++)
        {
            b[i] = Mathf.Atan2(1, 2 ** -i);
        }
    }
    public float calculate_K(int n)
    {
        float k = 1.0f;
        for(int i = 0; i < n; i++) 
        {
            k *= 1/Mathf.sqrt(1 + Mathf.Pow(2, -i * 1.0f));
        }
        return k;
    }
    public (float x, float y) cordic(float alpha, int n)
    {
        float[] theta_table = generateTheta(n);
        float k = calculate_K(n);
        float theta = 1.0f;
        float x = 1.0f; 
        float y = 0f; 
        int sigma = 1;
        float b_1 = 1;
        foreach(float t: theta_table)
        {
            sigma = sigma > alpha ? sigma + 1 : -1;
            theta += sigma * t; 
            x, y = x - sigma * y * b_1, sigma * b_1 * x + y; 
            b_1 << 1;
        }
        return (x * k), (y * k);
    }
}