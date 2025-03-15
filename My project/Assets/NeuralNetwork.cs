using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    const float e = 2.7182818284590452353602874713527f;
    const float pi = 3.1415926535897932384626433832795028841971f;
    public float[] bias;
    public int length;
    public float[] weights;
    public float[] features;
    public float output; 
    public float learning_rate;
    public int iteration;
    public void Start()
    {
        features[0] = features[0] + gaussian_noise(Random.Range(0.001f, 1f), Random.Range(0.100f, 1f), Random.Range(0.11f, 1.11f));
        for(int i = 0; i < iteration; i++){
            Forward_Propgation();
            print("Connection of the Neural Network is" + features[length - 1]);
            Backpropgation(output, learning_rate);
            Forward_Propgation();
            print("Network after Backpropgation is" + features[length - 1]);
        }
    }
    public void Forward_Propgation(){
        for(int i = 1; i <= length; i++){
            features[i] = weights[i - 1] * features[i - 1] + bias[i - 1];
        }
        return;
    }
    public float cross_entropy_loss(float x, float output)
    {
        return Mathf.Log(output - x);
    }
    public void Backpropgation(float output, float learning_rate)
    {
        int next = 0;
        for(int i = 0; i < length - 1; i++)
        {
            float loss = cross_entropy_loss(features[next], output);
            float delta = loss/output;
            weights[i] = weights[i] - learning_rate * delta;
            bias[i] = bias[i] - learning_rate * delta;
        }
    }
    public float gaussian_noise(float std_devi, float mean_grey, float grey)
    {
        return (1/(std_devi * (2 * pi))) * Mathf.Pow(e, Mathf.Pow(grey - mean_grey, 2)/Mathf.Pow(2 * std_devi, 2));
    }
}
