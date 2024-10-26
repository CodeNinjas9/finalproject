using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rendering : MonoBehaviour
{
    MeshFilter[] meshFilters; 
    CombineInstance[] combine;
    const float e = 2.7182818284590452353602874713527f;
    float id; 
    int time_steps;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float generation_function(float id, int time_step)
    {
        return Mathf.Pow(e, id) * time_step;
    }
    // Update is called once per frame
    void Update()
    {

    }
    Mesh Combining_Meshes(int lenght)
    {
        combine = new CombineInstance[lenght];
        for(int i = 0; i < lenght; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
    void LayerGeneration(Mesh mesh)
    {
        int i = 0; 
        while(mesh.vertices[i] != null)
        {
            mesh.vertices[i] += Vector3.down; 
            i++;
        }
        mesh.RecalculateBounds();
    }
    void RenderLowerDetails(Mesh mesh, int time_step)
    {
        int i = 0;
        while(mesh.vertices[i] != null)
        {
            int prev = (i - time_step);
            mesh.vertices[i] += mesh.vertices[prev];
            mesh.vertices[prev] = Vector3.zero;
        }
        mesh.RecalculateBounds();
    }
}

