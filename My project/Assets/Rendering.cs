using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rendering : MonoBehaviour
{
    public MeshFilter[] meshFilters; 
    public int length;
    public int[] generationId;
    public Vector3 cameraposition;
    public MeshFilter a;
    public MeshFilter ano;
    CombineInstance[] combine;
    // Start is called before the first frame update
    void Start()
    {
        a.mesh = Combining_Meshes(length, 2, 3);
        a.mesh = RenderLowerDetails(a.mesh, 1, 5);
        a.mesh = CameraFacing(cameraposition, 6, a.mesh, 1, 1, 1);
        ano.mesh = CameraFacing(cameraposition, 6, a.mesh, 1, 1, 1);
    }
    Mesh Combining_Meshes(int lenght, int xtime_step, int ztime_step)
    {
        combine = new CombineInstance[lenght];
        for(int i = 0; i < lenght; i++)
        {
            combine[i].mesh = meshFilters[i].mesh;
            if(i % xtime_step == 0){
                meshFilters[i].transform.position += new Vector3(0, 0, -1);
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                continue;
            }
            if(i % ztime_step == 0)
            {
                meshFilters[i].transform.position += new Vector3(0, 1, 0);
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                continue;
            }
            meshFilters[i].transform.position += new Vector3(-1, -1, -1);
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
    Mesh CameraFacing(Vector3 cameraPos, int len, Mesh mesh, int boundX, int boundY, int boundZ)
    {
        Vector3[] vert = new Vector3[len];
        Vector3[] normals = new Vector3[len];
        int[] triangles = new int[len];
        for(int i = 0; i < len; i++)
        {
            switch(cameraPos.z - mesh.vertices[i].z < boundZ || cameraPos.x - mesh.vertices[i].z < boundX || cameraPos.y - mesh.vertices[i].y < boundY)
            {
                case true:
                    vert[i] = mesh.vertices[i];
                    normals[i] = mesh.normals[i];
                    triangles[i] = mesh.triangles[i];
                    break;
                case false:
                    break;
            }
        }
        mesh.Clear();
        mesh.triangles = triangles;
        mesh.vertices = vert;
        mesh.normals = normals;
        return mesh;
    }
    Mesh RenderLowerDetails(Mesh mesh, int time_step, int a_length)
    {
        for(int i = 1; i < a_length; i+=time_step)
        {
            int prev = (i - time_step);
            mesh.vertices[i] += mesh.vertices[prev];
            mesh.normals[i] += mesh.normals[prev];
            mesh.vertices[prev] = Vector3.zero;
            mesh.normals[prev] = Vector3.zero;
        }
        mesh.RecalculateBounds();
        return mesh;
    }
}

