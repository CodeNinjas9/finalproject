public class ImprovedGameOfLife: MonoBehaviour
{
    ulong bitstring;
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
                ulong l, i = full_adder(a, b, c);
                ulong m, j = full_adder(d, e, f);
                ulong n, k = half_adder(g, h);
                ulong y, w = full_adder(i, j, k);
                ulong x, z = full_adder(l, m, n);
                result |= w;
                result &= (y ^ z);
                result &= ~x;
                new_array[v] = result;
            }
        }
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