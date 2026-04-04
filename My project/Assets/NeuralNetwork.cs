using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    const float pi = 3.1415926535;
    const float euler = 2.7182818284590452353602874713527;
    public float[] bias;
    public int length;
    public float[] weights;
    public float[] features;
    public float output; 
    public float learning_rate;
    public int iteration;
    void Start()
    {
        for(int i = 0; i < iteration; i++){
            Forward_Propgation();
            print("Connection of the Neural Network is" + features[length - 1]);
            Backpropgation(output, learning_rate);
            Forward_Propgation();
            print("Network after Backpropgation is" + features[length - 1]);
        }
    }
    void Update()
    {

    }
    void Forward_Propgation()
    {
        int next;
        for(int i = 0; i < length - 1; i++)
        {
            next = i + 1;
            features[next] = weights[i] * features[i] + bias[i];
        } 
    }
    float cross_entropy_loss(float x, float output)
    {
        return Mathf.Log(output - x);
    }
    void Backpropgation(float output, float learning_rate)
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
    float gaussian_noise(float mean_grey, float std_devi, float grey)
    {
        return ((1/std_devi) * (2 * (Mathf.sqrt(pi)))) * (Mathf.pow(e, Mathf.pow(-(grey - mean_grey)/ 2)/Mathf.pow(2 * std_devi)))
    }
}
