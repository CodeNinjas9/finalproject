public class CORDIC: MonoBehaviour
{
    public float[] atan_table;
    public (float x, float y) cordic(float input_value, int n, float[] p)
    {
        float theta = 0.0f;
        float x = 1.0f;
        float y = 0;
        float k = 0.6072529350088812561694;
        float sigma = 1.0f;
        int z = 1.0f;
        foreach(float angle in p)
        {
            input_value > theta ? sigma = 1 : sigma = -1;
            theta += sigma * angle;
            float x1, y1 = x - sigma * y * z, sigma * z * x + y;
            x = x1;
            y = y1;
            z >> 1;
        }
        return (x * k), (y * k);
    }
}