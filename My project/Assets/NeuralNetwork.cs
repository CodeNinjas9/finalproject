using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    float[] bias;
    int length;
    float[] weights;
    float[] features;
    void Start()
    {

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
        for(int i = 0; i < length; i++)
        {
            float loss = cross_entropy_loss(features[next], output);
            float delta = loss/output;
            weights[i] = weights[i] - learning_rate * delta;
            bias[i] = bias[i] - learning_rate * delta;
        }
    }
}
