using System;
public class Rope {
float rope_mask_scalar(int pos, float[] freqs, int max_len) {
    float sum = 0.0f;
    for (int i = 0; i < freqs.size(); i++) {
        float theta = pos * freqs[i] * Math.pow(float(pos)/float(max_len)*2.0f + 1e-6f, 0.25f);
        sum += (cosf(theta) + 1.0f) * 0.5f;  
    }
    return sum / freqs.size();
}
float rope_mask_3d(int x, int y, int z, float[] freqs, int max_len) 
{
    float mx = rope_mask_scalar(x, freqs, max_len);
    float my = rope_mask_scalar(y, freqs, max_len);
    float mz = rope_mask_scalar(z, freqs, max_len);
    return (mx + my + mz) / 3.0f;  
}
float arange(int start, int len, int step)
{
    float[] x; 
    for(int i = start; i < len; i += step)
    {
        x[i] = i; 
    }
    return x;
}
float clamp(float x, float max, float min)
{
    if(x < min) {return min;}
    if(x > max) {return max;}
    return x;
} 
}