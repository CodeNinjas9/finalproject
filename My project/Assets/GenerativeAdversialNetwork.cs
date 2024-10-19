using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerativeAdversialNetwork : MonoBehaviour
{
    const float e = 2.7182818284590452353602874713527f;
    public NeuralNetwork discrimantor;
    public NeuralNetwork candidate_generator;
    public int iterations;
    public float[] data_set;
    public int length;
    // Start is called before the first frame update
    void Start()
    {
        float mean = find_mean(data_set, length);
        float std_devi = find_standardDeviation(data_set, length, mean);
    }
    float find_mean(float[] array, int length)
    {
        float mean = 0;
        foreach(float item in array)
        {
            mean += item;
        }
        return mean/length;
    }
    float find_standardDeviation(float[] array, int length, float mean)
    {
        float seq = 0;
        foreach(float item in array)
        {
            seq += (item - mean);
        }
        return (Mathf.Sqrt(seq)/length);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    float discrimantor_output_calcluator(float network_output, float discimrnator_output)
    {
        return 0f;
    }
    float gaussian_distrubtion(float x, float mean, float std_devi)
    {
        return (1/Mathf.Sqrt(2 * Mathf.Pow(std_devi, 2))) * (Mathf.Pow(e, -(x - mean)/Mathf.Pow(2 * std_devi, 2)));
    }
}
