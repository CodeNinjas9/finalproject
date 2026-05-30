using System;
using UnityEngine;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using UnityEditor.ShaderGraph.Internal;
using System.Data;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class BinaryGreedyMesher: MonoBehaviour
{
    
    [System.Serializable]
    public struct VoxelGrid
    {
        public uint[] bits;
        public byte blocktype;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int countTrailingZeros(uint n)
    {
        if(n == 0) {return 32;}
        int ctn = 0;
        while((n & 1) == 0)
        {
            n >>= 1;
            ctn++;
        }
        return ctn;
    }

    public VoxelGrid faceCulling(VoxelGrid a, VoxelGrid b, int len)
    {
        for(int i = 0; i < len - 1; i++)
        {
            a.bits[i] &= ~b.bits[i + 1];
        }
        return new VoxelGrid {bits = a.bits, blocktype = a.blocktype};
    }
    public void buildRepr(int len)
    {
        uint[] k = new();
        uint[] k_masks = new();
        for(int y = 0; y < len + 2; y++)
        {
            for(int x = 0; x < len + 2; x++)
            {
                for(int z = 0; z < len + 2; z++)
                {
                    
                }
            }
        }
    }
    public List<(uint a, uint b, uint c, uint d)> greedySilce(VoxelGrid a, int m1, int m2)
    {
        List<(uint a, uint b, uint c, uint d)> k = new();
        for(uint i = 0; i < 32; i++)
        {
            int y, h, w;
            y = 0;
            h = 0;
            w = 0;
            while (y < m1)
            {
                
                y += countTrailingZeros((a.bits[i] << y));
                h += countTrailingZeros(~(a.bits[i] << y));
                uint mask = (h >= 0 && h < 32) ? ((1U << h) - 1) : uint.MaxValue;
                mask <<= y;
                if(y <= m1)
                {
                    continue;
                }
                w = 1;
                while (i + w < m2)
                {
                    uint next_row = (a.bits[i + w] >> y) & mask;
                    if (next_row != mask)
                    {
                        break;
                    }
                    a.bits[i + w] &= ~mask;
                    w+=1;
                }
                k.Add(((uint)y , (uint)h, (uint)w, i)); 
                y += h;
            }
        }
        return k;
    }
}