using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ImprovedGameOfLife
{
    ulong[] gameOfLife(ulong[] bitstring, int iters, int elements, int row_size)
    {
        ulong[] new_array = bitstring;
        for(int i = 0; i < iters; i++)
        {
            for(int v = 0; v < elements; v++)
            {
                ulong result = bitstring[v];
                ulong a = bitstring[v - row_size];
                ulong b = bitstring[v + row_size];
                ulong c = (bitstring[v - row_size] >> 4) | (bitstring[v - row_size - 1] << 60);
                ulong d = (bitstring[v - row_size] << 4) | (bitstring[v - row_size + 1] >> 60);
                ulong e = (bitstring[v + row_size] >> 4) | (bitstring[v + row_size + 1] << 60);
                ulong f = (bitstring[v + row_size] << 4) | (bitstring[v + row_size - 1] >> 60);
                ulong g = 0; 
                ulong h = 0; 
                (ulong, ulong) t = full_adder(a, b, c); // l, i
                (ulong, ulong) m = full_adder(d, e, f); // m, j
                (ulong, ulong) k = half_adder(g, h); // n, k
                (ulong, ulong) y = full_adder(t.Item2, m.Item2, k.Item2); // y, w
                (ulong, ulong) x = full_adder(t.Item1, m.Item1, k.Item1); // x, z
                result |= y.Item2;
                result &= (y.Item1 ^ x.Item2);
                result &= ~x.Item2;
                new_array[v] = result;
            }
        }
        return new_array;
    }
    (ulong, ulong) half_adder(ulong a, ulong b)
    {
        ulong sum = a ^ b;
        ulong carry = a & b;
        return (sum, carry);
    }
    (ulong, ulong) full_adder(ulong a, ulong b, ulong c)
    {
        ulong temp = a ^ b;
        ulong sum = temp ^ c;
        ulong carry = (a & b) | (temp & c);
        return (sum, carry);
    }
}