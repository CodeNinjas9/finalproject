using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float base_exp;
    public float[] array;
    public float length;
    public int iteration;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < iteration; i++)
        {
            print("Generating function has been solved" + solve_generating_functions(iteration));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // Solving using the roots of unity of that particular sequence
    float solve_generating_functions(int sequence)
    {
        return 1/sequence * (Mathf.Pow(base_exp, length)) + (sequence - 1) * (Mathf.Pow(base_exp, length/sequence));
    }
}