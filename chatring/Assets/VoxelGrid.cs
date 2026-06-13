using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
public class VoxelGrid : MonoBehaviour
{
    public Vector3Int dimensions = new Vector3Int(32, 32, 32);
    public Vector3Int GetVoxelFromWorldPoint(UnityEngine.Vector3 worldPoint)
    {
        UnityEngine.Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
        int x = Mathf.FloorToInt(localPoint.x / 1.0f);
        int y = Mathf.FloorToInt(localPoint.y / 1.0f);
        int z = Mathf.FloorToInt(localPoint.z / 1.0f);
        x = Mathf.Clamp(x, 0, 32 - 1);
        y = Mathf.Clamp(y, 0, 32 - 1);
        z = Mathf.Clamp(z, 0, 32 - 1);
        return new Vector3Int(x, y, z);
    }
    public Vector3Int handleVoxelCollision(UnityEngine.Vector3 worldHit)
    {
        return GetVoxelFromWorldPoint(worldHit);
    }
}