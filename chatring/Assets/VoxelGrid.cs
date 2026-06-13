using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
public class VoxelGrid : MonoBehaviour
{
    public Vector3Int dimensions = new Vector3Int(32, 32, 32);
    public Vector3Int GetVoxelFromWorldPoint(Vector3 worldPoint)
    {
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
        int x = Mathf.FloorToInt(localPoint.x / voxelSize);
        int y = Mathf.FloorToInt(localPoint.y / voxelSize);
        int z = Mathf.FloorToInt(localPoint.z / voxelSize);
        x = Mathf.Clamp(x, 0, gridDimensions.x - 1);
        y = Mathf.Clamp(y, 0, gridDimensions.y - 1);
        z = Mathf.Clamp(z, 0, gridDimensions.z - 1);
        return new Vector3Int(x, y, z);
    }
    public Vector3Int handleVoxelCollision(Vector3 worldHit)
    {
        return GetVoxelFromWorldPoint(worldHit);
    }
}