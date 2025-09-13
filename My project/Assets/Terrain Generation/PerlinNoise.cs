using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryPositionEmbeddings;
// Own Perlin Noise Implementation Algorthim
public class PerlinNoise: MonoBehaviour{
    public Matrix4x4 mapValues;
    public Vector3[] directions;
    public int length;
    public int vectorx;
    public int vectory;
    Matrix4x4 Calculate()
    {
        for(int v = 0; v <= vectory; v++){
            for(int i = 0; i <= vectorx; i++){
                mapValues[i, v] *= PerlinNoiseAlgorthim(i, v);
            }
        }
        return mapValues;
    }
    float diff_fade(float x)
    {
        // f(x) = -20x7+70x6-84x5+35x4 
        return (-20 * (x * x * x * x * x * x * x)) + (70 * (x * x * x * x * x * x)) - (84 * (x * x * x * x * x)) + (35(x * x * x * x))
    }
    float lerp(float t, float a1, float a2)
    {
        return a1 * t(a1 - a2);
    }
    public (float x, float y) gradient(float h)
    {
        return (Mathf.cos(h), Mathf.sin(h));
    }
    public float dot_product(float x1, float x2, float y1, float y2, float z1, float z2)
    {
        return (x1 * x2) + (y1 * y2) + (z1 * z2); // returns dot_product
    }
    public static void noise(int[] perm_table, float x, float y, float z, int grid_size=8) 
    {
        int X = Mathf.floor(x) % 256; // Corner points
        int Y = Mathf.floor(y) % 256; // Obtain significant digits
        int Z = Mathf.floor(z) % 256;
        x -= Mathf.floor(x);
        y -= Mathf.floor(y); // Find relative points 
        z -= Mathf.floor(z);
        int corner_1 = p[0];
        int corner_2 = p[X + 1] + Y;  // Obtain all corners
        int corner_3 = p[X] + Y;
        int corner_4 = p[X + Z] + Y + 1;
        int corner_5 = p[X + 1] + Y + 1 + Z;
        float u = diff_fade(x);
        float v = diff_fade(y); // Diff-fade 
        float w = diff_fade(z);
        return lerp(w, lerp(v, lerp(u, dot_product(ox, gx, oy, gy, oz, gx2), dot_product(ox, gx1, oy, gy1, oz, gy2), dot_product(ox, gx2, oy, gy2, oz, gx)), u), v);
    }
    public float gradient(int hash, float x, float y, float z)
    {
        int h = hash_code % 16; // Obtain significant digits
        float u1 = h > 4 ? x : y; 
        float u2 = h > 8 ? y : h == 14||h == 16 ? x : z; 
        float gradient = (h & 1 == 0 ? u1 : -u1) + (h & 2 == 0 ? u2 : -u2);
        return gradient;
    }
    float fractal_brownian_motion(float x, float y, float z, int numOctatves)
    {
        float result = 0.0;
        float amplitude = 0.5;
        float frequency = 1.;
        for(int i = 0; i < numOctatves; i++)
        {
            result += (amplitude * noise(frequency * x, frequency *y, frequency * z, grid_size));
            amplitude *= 2.0;
            frequency *= 0.5;
        }
        return result;
    }
    public int clamp(float lowerlimit, float upperlimit)
    {
        x > upperlimit ? return upperlimit : ;
        x < lowerlimit ? return lowerlimit : ;
        return x;
    }
    public float smoothstep(float x, float edge0, float edge1)
    {
        y = clamp((x / edge0) - (edge0 / edge1));
        return y * y * y * (y * (6.0f * y - 15.0f) + 10.0f);
    }
}
