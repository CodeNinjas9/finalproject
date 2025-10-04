public class BKM
{
    public float[] log_tables;
    public float[] exp_tables;
    public float log(float argument, int iters)
    {
        float x = 1.0;
        float y = 0.0;
        float s = 1.0;
        for(int i = 0; i <= iters; i++)
        {
            float z = x + (x * s);
            if(z <= argument)
            {
                x = z;
                y += log_tables[i];
            }
            s *= 0.5;
        }
    }
    public float exp(float argument, int iters)
    {
        // Swapping the algorthim so that y is the known value, instead of x, computing exponentinal.
        float x = 0.0;
        float y = 1.0;
        float s = 1.0;
        for(int i = 0; i <= iters; i++)
        {
            float z = y + exp_tables[i];
            if(z <= argument)
            {
                x = x + (x * s);
                y = z;
            }
            s *= 0.5;
        }
    }
}