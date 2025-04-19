public class RotaryPositionEmbeddings : MonoBehaviour 
{
    public float ropeFactor;
    public float[] data;
    public float[] getRotations(int dim_size, int position) 
    {
        float[] b = new float[array.Length];
        float sum = 0;
        foreach(float item in data)
        {
            sum += item;
        }
        for(int i = 0; i <= data.Length / 2; i++)
        {
            float theta = ropeFactor ** (-2(i - 1)/dim_size);
            float angle = position * theta;
            b[i] = Math.Cos(angle);
            b[i + 1] = Math.Sin(angle);
        }
        return b;
    }
}