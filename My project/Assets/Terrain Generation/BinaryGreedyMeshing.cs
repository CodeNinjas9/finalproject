public class BinaryGreedyMeshing
{
    public ulong[] GreedyMeshing(ulong[] voxels, ulong slice_size)
    {
        foreach(ulong block in voxels)
        {
            ulong[] slices; 
            ulong _ = block & (slice_size - 1); // ensure slice_size is even
            for(int i = 0; i < _; i++)
            {
                ulong pointer = 0;
                ulong trailing_zeros = (block & ~0) - (block) 
                ulong skip_zeroes = block >> (trailing_zeros) // deappends first zeroes
                pointer += (skip_zeroes);
                ulong slice_type = ((block >> i * _) &  0) & (block); // generates mask
                slices[i] = slice_type;
            }
            ulong last_merged_slice_index = 0; 
            ulong[] finished_bits = 0;
            for(int k = 0; k < _; k++)
            {
                if(slices[last_merged_slice_index] == slices[k])
                {
                    preserved = slices[last_merged_slice_index];
                    last_merged_slice_index = k;
                    mask = ~(slices[last_merged_slice_index]); // gets zero bits
                    mask = (slices[last_merged_slice_index] >> mask);
                    g_mask = ~(slices[k]);
                    g_mask = (slices[k] >> g_mask);
                    slices[last_merged_slice_index] = slices[last_merged_slice_index] & !mask; // nukes last bits
                    slices[k] = (g_mask + mask);
                }
            }
        }
        
    }
}