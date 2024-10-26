using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationTheory : MonoBehaviour
{
    const float e = 2.7182818284590452353602874713527f; 
    public float output;
    public float learning_rate;
    public int length;
    public float[] keys;
    public float[] query;
    float sigmoidCurve(float x, float mean)
    {
        return (mean * 2) * (1/1 + Mathf.Pow(e, -x));
    }
    // Start is called before the first frame update
    void Start()
    {
        float sum1 = 0;
        float sum2 = 0;
        float iter2 = 0;
        float iter1 = 0;
        foreach(float key in keys)
        {
            sum1 += (key);
            iter1++;
        }
        float mean1 = (sum1/iter1);
        foreach(float q in query)
        {
            sum2 += (q);
            iter2++;
        }
        float mean2 = (sum2/iter2);
        for(int i = 0; i < length - 1; i++)
        {
            float result = Mathf.Round(sigmoidCurve(keys[i], mean1));
            if(result == 0f)
            {
                keys[i] = 0;
            }
        }
        for(int i = 0; i < length - 1; i++)
        {
            float result = Mathf.Round(sigmoidCurve(query[i], mean2));
            if(result == 0f)
            {
                query[i] = 0f;
            }
        }
        float sum = 0;
        for(int i = 0; i < length - 1; i++)
        {
            if(keys[i] == 0f)
            {
                continue;
            }
            if(query[i] == 0f)
            {
                continue;
            }
            float probablity = probablity_calculator(keys[i], query[i]);
            float distrub = distrubtion_calculator(probablity, length);
            float s_output = entropy_calculator(distrub, 2);
            print(probablity);
            print(distrub);
            print(s_output);
            sum += s_output; 
        }
        for(int i = 0; i < length - 1; i++)
        {
            gradient_ascent(output, sum, learning_rate);
        }      
    }
    float entropy_calculator(float p, float information_id)
    {
        return (p * -Mathf.Log(p, information_id));
    }
    float cross_entropy_loss(float probablity, float output)
    {
        return Mathf.Log(probablity - output);
    }
    float distrubtion_calculator(float x, int time_steps)
    {
        return Mathf.Pow(e, x) * time_steps;
    }
    float probablity_calculator(float key, float query)
    {
        float probablity; 
        probablity = query * cross_entropy_loss(key, output);
        return probablity;
    }
    // Updates the keys and values based on cross_entropy_loss
    void gradient_ascent(float output, float vt_output, float learning_rate)
    {
        float delta = (output - vt_output);
        for(int i = 0; i < length - 1; i++)
        {
            if(keys[i] == 0)
            {
                continue;
            }
            if(query[i] == 0)
            {
                continue;
            }
            keys[i] = keys[i] + (learning_rate * delta/output);
            query[i] = query[i] + (learning_rate * delta/output);
        }
    }
    // Update is called once per frame
    void Update()
    {
                
    }
}
