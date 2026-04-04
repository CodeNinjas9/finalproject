using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using Unity.Collections;
using System.Collections.Generic;

public class Manager: MonoBehaivor
{
    public Material worldMat;
    public Vector3 initpos;

    public void Start()
    {
        Gameobject b = new Gameobject("xkkakaka");
        b.addComponent<VoxelCube>();
        VoxelCube k = b.GetComponent<VoxelCube>();
        k.GenerateMesh();
        k.UploadMesh();        
    }    
}