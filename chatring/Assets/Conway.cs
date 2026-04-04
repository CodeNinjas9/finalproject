using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conway : MonoBehaviour
{
    public bool[] arrays; 
    public int dim; 
    public int row_size; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int neighbors = 0; 
        for(int i = 0; i < dim; i++)
        {
            if(i + 1 > dim)
            {
                neighbors += (arrays[i - row_size - 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size] == true ? 1 : 0);
                neighbors += (arrays[i - 1] == true ? 1 : 0);
            }
            else if(i + row_size > dim)
            {
                neighbors += (arrays[i + 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size - 1] == true ? 1 : 0);
                neighbors += (arrays[i - 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size + 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size] == true ? 1 : 0);
            }
            else if(i - row_size < 0)
            {
                neighbors += (arrays[i + 1] == true ? 1 : 0);
                neighbors += (arrays[i - 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size + 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size] == true ? 1: 0);
                neighbors += (arrays[i + row_size - 1] == true ? 1 : 0);
            }
            else if(i - 1 < 0)
            {
                neighbors += (arrays[i + 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size + 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size] == true ? 1 : 0);
            }
            else
            {
                neighbors += (arrays[i - 1] == true ? 1 : 0);
                neighbors += (arrays[i + 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size + 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size - 1] == true ? 1 : 0);
                neighbors += (arrays[i + row_size] == true ? 1 : 0);
                neighbors += (arrays[i - row_size + 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size - 1] == true ? 1 : 0);
                neighbors += (arrays[i - row_size] == true ? 1 : 0);
            }
            
        }

    }
}
