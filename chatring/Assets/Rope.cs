using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rope: MonoBehaviour {
public float rope_mask_scalar(int pos, float[] freqs, int max_len) {
    float sum = 0.0f;
    for (int i = 0; i < freqs.Length; i++) {
        float theta = pos * freqs[i] * (float)Math.Pow((float)pos/(float)(max_len)*2.0f + 1e-6f, 0.25f);
        sum += (float)(Math.Cos(theta) + 1.0f) * 0.5f;  
    }
    return sum / freqs.Length;
}
public float rope_mask_3d(int x, int y, int z, float[] freqs, int max_len) 
{
    float mx = rope_mask_scalar(x, freqs, max_len);
    float my = rope_mask_scalar(y, freqs, max_len);
    float mz = rope_mask_scalar(z, freqs, max_len);
    return (mx + my + mz) / 3.0f;  
}
public float clamp(float x, float max, float min)
{
    if(x < min) {return min;}
    if(x > max) {return max;}
    return x;
} 
}