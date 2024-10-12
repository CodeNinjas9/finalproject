using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
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
}
