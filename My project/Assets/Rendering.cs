using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rendering : MonoBehaviour
{
    public Mesh[] meshFilters; 
    CombineInstance[] combine;
    public Matrix4x4 blockRendering;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Matrix4x4 createBlocks(Matrix4x4 blockRendering, int xtime_step, int ztime_step)
    {
        switch(blockRendering[13] % xtime_step == 0, blockRendering[33] % ztime_step == 0){
            case (true, false):
                blockRendering[33] += 1;
                blockRendering[13] = 0;
                break;
            case (false, true):
                blockRendering[23] += 1;
                blockRendering[33] = 0;
                break;
        }
        return blockRendering;
    }
    Mesh Combining_Meshes(int lenght, int xtime_step, int ytime_step)
    {
        combine = new CombineInstance[lenght];
        for(int i = 0; i < lenght; i++)
        {
            combine[i].mesh = meshFilters[i];
            combine[i].transform = createBlocks(blockRendering, 10, 10);
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
    void CameraFacing(Vector3 cameraPos, int len, Mesh mesh)
    {
        for(int i = 0; i < len; i++)
        {
            switch(cameraPos.x > 0 && mesh.vertices[i].x > 0, cameraPos.y > 0 && mesh.vertices[i].y > 0, cameraPos.z > 0 && mesh.vertices[i].z > 0)
            {
                case (true, true, true):
                    break;
                case (true, false, false):
                    mesh.vertices[i] = Vector3.zero;
                    break;
                case (false, true, true):
                    break;
                case (true, true, false):
                    mesh.vertices[i] = Vector3.zero;
                    break;
                case (true, false, true):
                    mesh.vertices[i] = Vector3.zero;
                    break;
            }
        }
    }
    void RenderLowerDetails(Mesh mesh, int time_step)
    {
        int i = 1;
        while(mesh.vertices[i] != null && mesh.normals[i] != null)
        {
            int prev = (i - time_step);
            mesh.vertices[i] += mesh.vertices[prev];
            mesh.normals[i] += mesh.normals[prev];
            mesh.vertices[prev] = Vector3.zero;
            mesh.normals[prev] = Vector3.zero;
            i += time_step;
        }
        mesh.RecalculateBounds();
    }
}

