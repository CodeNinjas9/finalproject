// Cave Generation - Mountain Generation
using Unity;
using UnityEngine;
using System.Collections.Generic;
public class CelluarAutomata: MonoBehaviour
{
    float entropy = 0f;
    float latestDensity = 0f;
    float simliarityScore = 0f;
    bool[] GameOfLife(bool[] cells, int row_number)
    {
        int alive_neighbors = 0;
        int dead_neighbors = 0; 
        int v = 0; 
        foreach(bool item in cells)
        {
            v++;
            if(v > 0)
            {
               cells[v - 1] == true ? alive_neighbors++ : dead_neighbors++;
               cells[v + 1] == true ? alive_neighbors++ : dead_neighbors++;
               if(v > row_number)
               {
                    cells[v - row_number] == true ? alive_neighbors++ : dead_neighbors++;
                    cells[v - row_number - 1] == true ? alive_neighbors++ : dead_neighbors++;
                    cells[v - row_number + 1] == true ? alive_neighbors++ : dead_neighbors++;
                    cells[v + row_number] == true ? alive_neighbors++ : dead_neighbors++;
                    cells[v + row_number - 1] == true ? alive_neighbors++ : dead_neighbors++;
                    cells[v + row_number + 1] == true ? alive_neighbors++ : dead_neighbors++;
               }
            }
        }
        return cells;
    }
    bool[] generateRandomMap(int size)
    {
        int indice = 0;
        bool[] a = new bool[size];
        foreach(bool item in a)
        {
            indice++;
            float condition = Random.Range(0f, 1f);
            condition >= 0.5 ? a[indice] = true : a[indice] = false;
        }
        return a;
    }
    void updateStats(float alive_cells, float dead_cells, float previous_entropy)
    {
        latestDensity = alive_cells / (dead_cells + alive_cells);
        entropy = Mathf.Log(((alive_cells * (alive_cells / alive_cells + dead_cells)) * Mathf.Log((alive_cells / alive_cells + dead_cells), 2.718281828459045235360287471352662497757247093699959574966967627724076630353)), 2);
        simliarityScore = entropy * Mathf.Log(entropy/previous_entropy, 2);
    }
}