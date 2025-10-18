using System;
using System.Numerics;
using u64 = System.UInt64;
using i32 = System.Int32;
using f32 = System.Single;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBinaryGreedyMeshing : MonoBehaviour
{
    private const u64 a = 1UL;
    private const u64 b = 0UL;
    public u64[] Culler(int length, u64[] faces)
    {
        for (int i = 0; i < length - 1; i++)
            faces[i] &= ~faces[i + 1];
        return faces;
    }
    public u64[] ShiftRowLeft(int length, u64[] row)
    {
        u64[] result = new u64[length];
        u64 carry = 0;
        for (int i = 0; i < length; i++)
        {
            u64 w = row[i];
            u64 nextCarry = (w >> 63) & 1UL;
            result[i] = (w << 1) | carry;
            carry = nextCarry;
        }
        return result;
    }
    public u64[] ShiftRowRight(int length, u64[] row)
    {
        u64[] result = new u64[length];
        u64 carry = 0;
        for (int i = length - 1; i >= 0; i--)
        {
            u64 w = row[i];
            u64 nextCarry = (w & 1UL) << 63;
            result[i] = (w >> 1) | carry;
            carry = nextCarry;
        }
        return result;
    }
    public u64[] MaskRange(int start, int len, int words)
    {
        u64[] result = new u64[words];
        int end = start + len - 1;

        int startWord = start / 64;
        int endWord = end / 64;
        int startBit = start % 64;
        int endBit = end % 64;

        if (startWord == endWord)
        {
            u64 mask = ((1UL << (endBit - startBit + 1)) - 1) << startBit;
            result[startWord] = mask;
        }
        else
        {
            result[startWord] = ~0UL << startBit;
            for (int w = startWord + 1; w < endWord; w++)
                result[w] = ~0UL;
            result[endWord] = (1UL << (endBit + 1)) - 1;
        }

        return result;
    }
    public int FindFirstSetBit(u64 x)
    {
        if (x == 0) return -1;
        return TrailingZeroCount();
    }
    public int TrailingZeroCount()
    {
        return 0; 
    }
    public u64 NukeBits(u64 x) => 0;
    public void GreedyMesh2D(u64[][] slice, int H, int W)
    {
        bool[,] used = new bool[H, W * 64];
        for (int y = 0; y < H; y++)
        {
            for (int word = 0; word < W; word++)
            {
                u64 bits = slice[y][word];
                while (bits != 0)
                {
                    int bit = TrailingZeroCount();
                    int x = word * 64 + bit;

                    if (used[y, x])
                    {
                        bits &= bits - 1;
                        continue;
                    }
                    int width = 1;
                    while (x + width < W * 64)
                    {
                        int nxWord = (x + width) / 64;
                        int nxBit = (x + width) % 64;
                        bool nextSet = ((slice[y][nxWord] >> nxBit) & 1UL) != 0;
                        if (!nextSet || used[y, x + width]) break;
                        width++;
                    }
                    int height = 1;
                    bool full = true;
                    while (y + height < H && full)
                    {
                        for (int wx = 0; wx < width; wx++)
                        {
                            int nxWord = (x + wx) / 64;
                            int nxBit = (x + wx) % 64;
                            bool nextSet = ((slice[y + height][nxWord] >> nxBit) & 1UL) != 0;
                            if (!nextSet || used[y + height, x + wx])
                            {
                                full = false;
                                break;
                            }
                        }
                        if (full) height++;
                    }
                    for (int dy = 0; dy < height; dy++)
                        for (int dx = 0; dx < width; dx++)
                            used[y + dy, x + dx] = true;
                    bits &= bits - 1;
                }
            }
        }
    }
}