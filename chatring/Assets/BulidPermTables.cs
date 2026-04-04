using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

namespace PermTables{
public class BulidPermTables
{
    public bool[] chosen;  
    public int n = 256;
    public List<int> perm; 
    public int target_iter; 
    
    public BulidPermTables(int n, int target_iter){
        this.chosen = new bool[n];
        this.n = n; 
        this.target_iter = target_iter;
        this.perm = new List<int>(n);
    }
}
}
