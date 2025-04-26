using Unity;
using UnityEngine;
using System.Collections.Generic;
public class ChunkGeneration: MonoBehaviour{
    void Start() 
    {

    }
    void Update() 
    {

    }
    int[] GameOfLife(int playingIterations, int[] cells, int iteration)
    {
        if(iteration < playingIterations)
        {
            return cells; 
        }
        else 
        {
            foreach(int item in cells)
            {
                ;
            }
        }
        return null;
    }
}