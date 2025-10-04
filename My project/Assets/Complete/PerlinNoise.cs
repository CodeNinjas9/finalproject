using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PerlinNoise : MonoBehaviour
{
    public float[,] mapValues;
    public int length;
    public int vectorx;
    public int vectory;
    public int vectorz;
    float[,] Calculate()
    {
        for (int v = 0; v <= vectory; v++)
        {
            for (int i = 0; i <= vectorx; i++)
            {
                for(int k = 0; k <= vectorz; k++)
                {
                    mapValues[i, v, k] *= noise(i, v, k);
                }
            }
        }
        return mapValues;
    }
    float diff_fade(float x)
    {
        return (-20 * Mathf.Pow(x, 7)) + (70 * Mathf.Pow(x, 6)) - (84 * Mathf.Pow(x, 5)) + (35 * Mathf.Pow(x, 4));
    }
    float lerp(float t, float a1, float a2)
    {
        return a1 + t * (a2 - a1);
    }
    float grad(int hash, float x, float y, float z)
    {
        int h = hash % 16; 
        float u = (h < 8) ? x : y;
        float v = (h < 4) ? y : (h == 12 || h == 14 ? x : z);
        float res = (((h & 1) == 0) ? u : -u) + (((h & 2) == 0) ? v : -v);
        return res;
    }
    public float noise(float x, float y, float z, int grid_size = 8)
    {
        int X = Mathf.FloorToInt(x) % 256;
        int Y = Mathf.FloorToInt(y) % 256;
        int Z = Mathf.FloorToInt(z) % 256;
        float x = x - Mathf.Floor(x);
        float y = y - Mathf.Floor(y);
        float z = z - Mathf.Floor(z);
        float u = diff_fade(x);
        float v = diff_fade(y);
        float w = diff_fade(z);
        int[] p = PermTable();
        int A  = p[X] + Y;
        int AA = p[A] + Z;
        int AB = p[A + 1] + Z;
        int B  = p[X + 1] + Y;
        int BA = p[B] + Z;
        int BB = p[B + 1] + Z;
        float x1, x2, y1, y2;
        x1 = lerp(u, grad(p[AA], x, y, z),
                     grad(p[BA], x - 1, y, z));
        x2 = lerp(u, grad(p[AB], x, y - 1, z),
                     grad(p[BB], x - 1, y - 1, z));
        y1 = lerp(v, x1, x2);

        x1 = lerp(u, grad(p[AA + 1], x, y, z - 1),
                     grad(p[BA + 1], x - 1, y, z - 1));
        x2 = lerp(u, grad(p[AB + 1], x, y - 1, z - 1),
                     grad(p[BB + 1], x - 1, y - 1, z - 1));
        y2 = lerp(v, x1, x2);
        return (lerp(w, y1, y2) + 1f) / 2f; 
    }
    float fractal_brownian_motion(float x, float y, float z, int numOctaves, int grid_size = 8)
    {
        float result = 0.0f;
        float amplitude = 0.5f;
        float frequency = 1.0f;
        for (int i = 0; i < numOctaves; i++)
        {
            result += amplitude * noise(frequency * x, frequency * y, frequency * z, grid_size);
            amplitude *= 0.5f;   
            frequency *= 2.0f;   
        }
        return result;
    }
    public float clamp(float x, float lowerlimit, float upperlimit)
    {
        if (x > upperlimit) return upperlimit;
        if (x < lowerlimit) return lowerlimit;
        return x;
    }
    public float smoothstep(float edge0, float edge1, float x)
    {
        float t = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
    }
    int[] PermTable()
    {
        int[] perm = new int[512];
        for (int i = 0; i < 256; i++)
        {
            perm[i] = basePerm[i % basePerm.Length];
            perm[256 + i] = perm[i];
        }
        return perm;
    }
}