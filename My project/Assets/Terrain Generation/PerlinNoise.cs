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
    public static void noise(float x, float y, float z, int grid_size) 
    {
        int X = (int)(x / grid_size); // Corner points
        int Y = (int)(y / grid_size); 
        int Z = (int)(z / grid_size);
        float u = diff_fade(x);
        ox = (x - X); // Offset vector calculation
        oy = (y - Y);
        oz = (z - Z);
        gx, gy = gradient((float)X);
        gx1, gy1 = gradient((float)Y);
        gx2, gy2 = gradient((float)Z);
        return lerp(u, dot_product(ox, gx, oy, gy, oz, gx2), dot_product(ox, gx1, oy, gy1, oz, gy2), dot_product(ox, gx2, oy, gy2, oz, gx));
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
