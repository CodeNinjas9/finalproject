using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float seed;
    public float base_exp;
    public string[] map;
    public float length;
    public int iteration;
    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(1f, Mathf.Pow(2f, 10f));
        base_exp = Mathf.Log(seed);
        for(int i = 0; i < iteration; i++)
        {
            print("Generating function has been solved" + solve_generating_functions(iteration));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // Solving using the roots of unity
    float solve_generating_functions(int sequence)
    {
        return (1/sequence * ((Mathf.Pow(base_exp, length)) + (sequence - 1 * (Mathf.Pow(base_exp, length/sequence)))));
    }
}
