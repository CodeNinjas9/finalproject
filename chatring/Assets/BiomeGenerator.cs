using UnityEngine;
using System.Collections.Generic;

public class BiomeGenerator : MonoBehaviour
{
    public int seed = 1337;
    public float cellSize = 50f; // Controls biome size

    public enum BiomeType { Desert, Forest, Tundra, Plains }

    public struct BiomeSeed {
        public Vector2 position;
        public BiomeType type;
    }
    public BiomeType GetBiomeAt(float x, float y) {
        
        
        float minDist = float.MaxValue;
        BiomeSeed nearestSeed = new BiomeSeed();

        int cellX = Mathf.FloorToInt(x / cellSize);
        int cellY = Mathf.FloorToInt(y / cellSize);

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                BiomeSeed s = GetSeedForCell(cellX + i, cellY + j);
                float dist = Vector2.Distance(new Vector2(x, y), s.position);
                
                if (dist < minDist) {
                    minDist = dist;
                    nearestSeed = s;
                }
            }
        }
        return nearestSeed.type;
    }

    private BiomeSeed GetSeedForCell(int cx, int cy) {
        Random.InitState(seed + cx * 31 + cy * 7); 
        
        Vector2 pos = new Vector2(
            (cx + Random.value + 0.12f * Mathf.PerlinNoise(cx, cy)) * cellSize,
            (cy + Random.value + 0.12f * Mathf.PerlinNoise(cx, cy)) * cellSize
        );

        BiomeType type = (BiomeType)Random.Range(0, 4);

        return new BiomeSeed { position = pos, type = type };
    }
}
