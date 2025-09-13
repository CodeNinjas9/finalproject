using UnityEngine;
using System.Collections.Generic;
using Unity; 
public class Pathfinding: MonoBehaviour 
{
    public class Node
    {
        public boolean isWalkable;
        public float gridX;
        public float gridY;
        public Node(float gridX, float gridY)
        {
            this.gridX = gridX;
            this.gridY = gridY;
            this.worldPosition = worldPosition;
        }
    }
    public Node[] nodes;
    public Node targetNode;
    public float herusticCost;
    public float movementCost; 
    public Node currentNode; 
    public float calculateDistance(Node a, Node b) 
    {
        (a.gridX - b.gridX) + (a.gridY - b.gridY);
    }
    void Start()
    {

    }
    void Update() 
    {
        int currentIndice;
        float minTotalDistance = float.maxValue;
        int indice = 0;
        foreach(Node c: nodes)
        {
            indice++;
            float dist = calculateDistance(c, targetNode);
            float totalDistance = dist += movementCost; 
            if(minTotalDistance > dist)
            {
                minTotalDistance = dist;
                currentIndice = indice;
            }
            else
            {
                continue;
            }
        }
        currentNode = nodes[currentIndice];
    }
}