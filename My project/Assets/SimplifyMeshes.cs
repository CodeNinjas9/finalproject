using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplifyMeshes : MonoBehaviour
{
    public Mesh mesh;
    public float mininum;
    const float phi = 1.6181.618033988749894848204586467491852693169f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float entropyLoss(float x, float y){
        return Mathf.Log(x - y);
    }
    float findMininium(int length, int constant)
    {
        float sum = 0;
        for(int i = 0; i < length; i++)
        {
            float scopedVariable;
            scopedVariable = ((mesh.vertices[i].x  + mesh.vertices[i].y + mesh.vertices[i].z) - (mesh.vertices[i + 1].x + mesh.vertices[i + 1].y + mesh.vertices[i + 1].z));
            scopedVariable *= scopedVariable;
            sum = sum + (scopedVariable * constant);
        }
        return sum;
    }
    float penalize_large_vertices(float adjustment, float length)
    {
        return (adjustment * length);
    }
    float calculate_energy(float distance, float adjustment, float mininium)
    {
        return (distance) + (adjustment) + (mininium);
    }
    float calculate_distance(float simplistic_complex, float x)
    {
        float sum = 0;
        for(int i = 0; i < length; i++)
        {
            float result;
            result = ((mesh.vertices[i].x  + mesh.vertices[i].y + mesh.vertices[i].z) - (mesh.vertices[i + 1].x + mesh.vertices[i + 1].y + mesh.vertices[i + 1].z));
            result *= result;
            sum = result * (mesh.vertices[i].x + mesh.vertices[i].y + mesh.vertices[i].z * (phi * simplistic_complex))
        }
        return sum;
    }
    float gradient_descent(float x, float output, float learning_rate){
        x = x - (entropyLoss(x, output)/output * learning_rate);
        return x;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
