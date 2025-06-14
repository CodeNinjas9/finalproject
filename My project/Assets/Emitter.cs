
public class RayTracedAudio: MonoBehaviour
{
    public int maxBounces = 9;
    public int numRays;
    public int layerMask = (1 << 2) | (1 << 1);
    public GameObject player;
    public float occlusion; 
    public Dictionary<int, float[]> audioData;
    public List<float[]> bufferedData;
    void Start() 
    {
        
    }
    void FixedUpdate()
    {
        for(int i = 0; i <= numRays; i++)
        {
            Vector3 ray = Vector3.forward * Quaternion.Euler(0, (360/numRays) * i, 0);
            if(Physics.Raycast(ray, transform.TransformDirection(Vector3.forward), out hit, Vector3.Distance(ray, player.transform.position), layerMask))
            {
                occlusion += 1/numRays;
            }
        }
    }
    void ResampleDelayLines(int maxiter, int iterations, List<float[]> bufferedData) 
    { 
        int random = Random.Range(0, bufferedData.Length);
        bufferedData.Add(audioData[random]);
        if(iterations >= maxiter)
        {
            return bufferedData;
        }
        ResampleDelayLines(maxiter, iterations++, bufferedData);
    }
    void FeedbackDelayNetwork(int iterations, int maxiter, List<float[]> delayLines, int rows, int cols) 
    {
        if(iterations >= maxiter)
        {
            return delayLines;
        }
        Matrix4x4 randomMatrix = GenerateMatrix();
        for(int z = 0; z < delayLines.Length; z++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    delayLines[z][i + 1 * j + 1] *= randomMatrix[i][j];
                }
            }
        }
        foreach(var item in delayLines)
        {
            for(int z = 0; z <= delayLines.Length; z++)
            {
                for(int i = 0; i <= item.Length; i++)
                {
                    delayLines[z][i] *= item[i];
                }
            }
        }
        FeedbackDelayNetwork(iterations++, maxiter, delayLines);
    }
    public void GenerateMatrix()
    {
        Matrix4x4 randomMatrix = new Matrix4x4();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                randomMatrix[i, j] = Random.Range(minRange, maxRange);
            }
        }
        return randomMatrix; 
    }
}