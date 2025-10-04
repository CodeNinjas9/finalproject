using u64 = ulong;
using i32 = int;
using f32 = float;
using System.Numerics.BitOperations;
public class NewBinaryGreedyMeshing
{
    ulong a = 1;
    ulong b = 0;
    int word_count = 4; 
    // culler, checks if next face is not visible
    public u64[] culler(int length, u64[] faces) {
        for(int i = 0; i < length - 1; i++)
        {
            faces[i] = faces[i] & ~(faces[i + 1]);
        }
        return faces;
    }
    public u64[] shift_row_by_one_left(int length, u64[] row)
    {
        u64[] changed_array; 
        u64 carry = 0;
        for(int i = 0; i <= length; i++)
        {
            u64 w = in[i];
            u64 nextCarry = (w >> 63) & a;
            changed_array[i] = (w << 1) | carry;
            carry = nextCarry;
        }
        return changed_array;
    }
    // shifts row by right
    public u64[] shift_row_by_one_right(int length, u64[] row)
    {
        u64[] changed_array; 
        u64 carry = 0;
        for(int i = 0; i <= length; i++)
        {
            u64 w = in[i];
            u64 nextCarry = (w & a) << 63;
            changed_array[i] = (w << 1) | carry;
            carry = nextCarry;
        }
        return changed_array;
    }
    // fallback meshing
    void fallback_naive_meshing(u64[] slice, int H, int W, int max_width)
    {
        for(int x = 0; x < W; x++)
        {
            for(int y = 0; y < H; y++)
            {
                u64 word = slice[x][y];
                while(word)
                {
                    int b = TrailingZeroCount(word);
                    int k = word * 64 + b;
                    if(x < max_width)
                    {
                        // add logic for emitting mesh later
                    }
                }
                word &= (word - 1);
            }
        }
    }
    // calculates and returns masks from [start, end)
    u64 mask_range(int start, int len, int words)
    {
        int end = (start + len - 1);
        int start_width = (start / 64);
        int end_width = (end / 64);
        int start_mod = (start % 64);
        int end_mod = (end % 64);
        if(start_width == end_width)
        {
            u64 mask = ((end_mod - start_mod + 1) == 64) ? 1 : (((1 << end_mod - start_mod + 1)) - 1 << start_mod); 
            return mask;
        }
        else
        {
            out[start_width] = (~b << sb);
            for (int w = sw+1; w < end_width; ++w) { out[w] = ~b };
            out[end_width] = ( (end_mod+1) == 64 ) ? ~b : ((a << (end_mod+1)) - a);
        }
    }
    int find_first_set_bit(u64 in)
    {
        int idx = 0; 
        for(int i = start_row; i < 64; i++)
        {
            if(in[i] & a)
            {
                return i;
            }
        }
        return -1;
    }
    int find_consecutive_ones(u64 in_x)
    {
        u64 max; 
        for(int offset = 0; offset < 64; offset++){
            offset_mask = (1 << offset);
            for(int y = 0; y < popcount(in_x); y++)
            {
                u64 mask = (1 << y - offset); 
                u64 in_x = ((in_x & offset_mask) << y) & (mask);
                if(in_x > max)
                {
                    max = in_x;
                }
            }
        }
        return max;
    }
    u64 nuke_bits(u64 x)
    {
        return ~(x | ~0);
    }
    u64 merge_all_quads(u64[] quads)
    {
        u64 k; 
        for(int i = 0; i < length; i++)
        {
            k |= quads[i] | quads[i + 1];
        }
        return k;
    }
    u64[] pad(u64[] inp, int pad_length)
    {
        u64[] outp = [];
        u64 k = merge_all_quads(inp);
        k = (k >> pad_length);
        for(int i = 0; i < range; i++)
        {
            outp[i] = k & (1 << i | ~0);
        }
        return outp; 
    }
    u64[] greedy_meshing_forward_pass(u64[] quads, byte[] type, int axis, int queue_size, int word_size, int pad_length)
    {
        // fallback time herusitic
        int time_heuristic = merge_all_quads(pad(quads, pad_length))  & ~merge_all_quads(shift_row_by_one_left(quads));
        // axis-isolated
        // grid ordering: X+, X-, Y+, Y-, Z+, Z-
        int rows = queue_size / word_size;
        int length; 
        u64[] prev_quads = culler(quads);
        u64[] partitions = [] 
        foreach(u64 quad in quads)
        {
            u64 n = quad;
            bool can_extend = true;
            int shift_axis = 1;
            int iter = 0;
            int partition_count = 0; 
            while(can_extend == true)
            {
                iter++; 
                if(shift_axis > axis)
                {
                    break;
                }
                if(find_first_set_bit(prev_quads[iter]) == -1)
                {
                    can_extend = false;
                }
                axis_mask = (1 << word_count * shift_axis | ~0) // mask out ones that arent the adjacent axises
                n = merge_algorthim(n, axis_mask & prev_quads[iter]) // merge them
                prev_quads[iter] = axis_mask & nuke_bits(prev_quads[iter]) // nuke the merged axis
                if(iter > length)
                {
                    shift_row_by_one_right(prev_quads); // next_axis
                    shift_axis += 1;
                    continue;
                }
            }
            partition_count++;
            partitions[partition_count] = n;
        }
        int merge_length = length; 
        u64[] new_partitions = [];
        u64[] old_partitions = [];
        while(merge_length > 2 && new_partitions != prev_partitions)
        {
            u64[] temp; 
            for(int i = 0; i < merge_length - 1; i++)
            {
                temp[i] = merge_algorthim(partitions[i] << pad_length, partitions[i + 1] << pad_length);
            }
            merge_length /= 2;
            old_partitions = new_partitions;
            new_partitions = temp;
        }
        return new_partitions;
    }
    u64 merge_algorthim(u64 k, u64 m)
    {
        
        int count = popcount(k);
        int count_1 = popcount(m);
        return ((1 << find_consecutive_ones(k)) | ~0) | ((1 << find_consecutive_ones(m) | ~0)); // MERGE EVERYTHING
    }
}