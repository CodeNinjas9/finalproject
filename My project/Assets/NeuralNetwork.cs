using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    const float e = 2.7182818284590452353602874713527f;
    const float pi = 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821480865132823066470938446095505822317253594081284811174502841027019385211055596446229489549303819644288109756659334461284756482337867831652712019091456485669234603486104543266482133936072602491412737245870066063155881748815209209628292540917153643678925903600113305305488204665213841469519415116094330572703657595919530921861173819326117931051185480744623799627495673518857527248912279381830119491298336733624406566430860213949463952247371907021798609437027705392171762931767523846748184676694051320005681271452635608277857713427577896091736371787214684409012249534301465495853710507922796892589235420199561121290219608640344181598136297747713099605187072113499999983729780499510597317328160963185950244594553469083026425223082533446850352619311881710100031378387528865875332083814206171776691473035982534904287554687311595628638823537875937519577818577805321712268066130019278766111959092164201989f;
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
    public void Update()
    {

    }
    public void Forward_Propgation()
    {
        int next;
        for(int i = 0; i < length - 1; i++)
        {
            next = i + 1;
            features[next] = weights[i] * features[i] + bias[i];
        } 
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
    //Added gaussian_noise!
    public float gaussian_noise(float std_devi, float mean_grey, float grey)
    {
        return (1/(std_devi * (2 * pi))) * Mathf.Pow(e, Mathf.Pow(grey - mean_grey, 2)/Mathf.Pow(2 * std_devi, 2));
    }
}
