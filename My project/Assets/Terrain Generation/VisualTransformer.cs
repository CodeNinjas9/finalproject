using System;

public class ViT
{
    const int CHUNK_SIZE = 16;  
    const int PATCH_SIZE = 4;   
    const int DIM = 64;          
    static void EncodePosition(int px, int py, int pz, float[] outArr)
    {
        for (int i = 0; i < DIM / 6; i++)
        {
            float freq = (float)Math.Pow(10000.0, 2.0 * i / DIM);
            outArr[i * 6 + 0] = (float)Math.Sin(px * freq);
            outArr[i * 6 + 1] = (float)Math.Cos(px * freq);
            outArr[i * 6 + 2] = (float)Math.Sin(py * freq);
            outArr[i * 6 + 3] = (float)Math.Cos(py * freq);
            outArr[i * 6 + 4] = (float)Math.Sin(pz * freq);
            outArr[i * 6 + 5] = (float)Math.Cos(pz * freq);
        }
    }
    static void PatchEmbedding(byte[,,] voxels, int x0, int y0, int z0, float[] embedding)
    {
        int idx = 0;
        for (int dz = 0; dz < PATCH_SIZE; dz++)
            for (int dy = 0; dy < PATCH_SIZE; dy++)
                for (int dx = 0; dx < PATCH_SIZE; dx++)
                {
                    embedding[idx++] = voxels[z0 + dz, y0 + dy, x0 + dx];
                }
    }
    static void Attention(float[] Q, float[] K, float[] V, float[] output, int seqLen, int dim)
    {
        for (int i = 0; i < seqLen; i++)
        {
            for (int d = 0; d < dim; d++)
            {
                float sum = 0;
                for (int j = 0; j < seqLen; j++)
                {
                    float dot = 0;
                    for (int k = 0; k < dim; k++)
                        dot += Q[i * dim + k] * K[j * dim + k];
                    sum += dot;
                }
                output[i * dim + d] = sum;
            }
        }
    }
    static void TransformerBlock(float[] x, int seqLen, int dim,
                                 float[] wQ, float[] wK, float[] wV,
                                 float[] wFF1, float[] bFF1,
                                 float[] wFF2, float[] bFF2)
    {
        float[] Q = new float[seqLen * dim];
        float[] K = new float[seqLen * dim];
        float[] V = new float[seqLen * dim];
        for (int i = 0; i < seqLen; i++)
        {
            for (int d = 0; d < dim; d++)
            {
                float sumQ = 0, sumK = 0, sumV = 0;
                for (int k = 0; k < dim; k++)
                {
                    sumQ += x[i * dim + k] * wQ[d * dim + k];
                    sumK += x[i * dim + k] * wK[d * dim + k];
                    sumV += x[i * dim + k] * wV[d * dim + k];
                }
                Q[i * dim + d] = sumQ;
                K[i * dim + d] = sumK;
                V[i * dim + d] = sumV;
            }
        }
        float[] attOut = new float[seqLen * dim];
        Attention(Q, K, V, attOut, seqLen, dim);
        for (int i = 0; i < seqLen; i++)
        {
            for (int d = 0; d < dim; d++)
            {
                float ff = 0;
                for (int k = 0; k < dim; k++)
                    ff += attOut[i * dim + k] * wFF1[d * dim + k];
                ff = Math.Max(0.0f, ff + bFF1[d]);

                float ff2 = 0;
                for (int k = 0; k < dim; k++)
                    ff2 += ff * wFF2[d * dim + k];
                x[i * dim + d] += ff2 + bFF2[d]; 
            }
        }
    }
    public static void GenerateChunk(byte[,,] inputVoxels, float[,,] chunkOutput)
    {
        int patchesPerDim = CHUNK_SIZE / PATCH_SIZE;
        int seqLen = patchesPerDim * patchesPerDim * patchesPerDim;
        float[] patchEmbeddings = new float[seqLen * DIM];

        int idx = 0;
        for (int z = 0; z < CHUNK_SIZE; z += PATCH_SIZE)
            for (int y = 0; y < CHUNK_SIZE; y += PATCH_SIZE)
                for (int x = 0; x < CHUNK_SIZE; x += PATCH_SIZE)
                {
                    PatchEmbedding(inputVoxels, x, y, z, patchEmbeddings.AsSpan(idx * DIM, DIM).ToArray());
                    EncodePosition(x / PATCH_SIZE, y / PATCH_SIZE, z / PATCH_SIZE,
                                   patchEmbeddings.AsSpan(idx * DIM, DIM).ToArray());
                    idx++;
                }
        float[] wQ = new float[DIM * DIM];
        float[] wK = new float[DIM * DIM];
        float[] wV = new float[DIM * DIM];
        float[] wFF1 = new float[DIM * DIM];
        float[] bFF1 = new float[DIM];
        float[] wFF2 = new float[DIM * DIM];
        float[] bFF2 = new float[DIM];
        Random rng = new Random();
        void InitArray(float[] arr) { for (int i = 0; i < arr.Length; i++) arr[i] = (float)(rng.NextDouble() * 0.02 - 0.01); }
        InitArray(wQ); InitArray(wK); InitArray(wV);
        InitArray(wFF1); InitArray(wFF2); InitArray(bFF1); InitArray(bFF2);
        TransformerBlock(patchEmbeddings, seqLen, DIM, wQ, wK, wV, wFF1, bFF1, wFF2, bFF2);
        idx = 0;
        for (int z = 0; z < CHUNK_SIZE; z += PATCH_SIZE)
            for (int y = 0; y < CHUNK_SIZE; y += PATCH_SIZE)
                for (int x = 0; x < CHUNK_SIZE; x += PATCH_SIZE)
                {
                    float val = patchEmbeddings[idx * DIM];
                    for (int dz = 0; dz < PATCH_SIZE; dz++)
                        for (int dy = 0; dy < PATCH_SIZE; dy++)
                            for (int dx = 0; dx < PATCH_SIZE; dx++)
                            {
                                chunkOutput[z + dz, y + dy, x + dx] = val;
                            }
                    idx++;
                }
    }
}