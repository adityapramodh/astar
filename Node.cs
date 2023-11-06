using UnityEngine;

// Node Model Class
public class Node 
{
    public bool walkable;
    public Vector3 worldPosition; 
    public int gridX;
    public int gridY;
    public Node parent;

    // G Value
    public int gCost;

    // H Value - Heuristic Value
    public int hCost;

    // F Value
    public int fCost 
    {
        get 
        {
            return hCost + gCost; // F vakue = G Value + H value.
        }
    }
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable; // Is walkable
        worldPosition = _worldPosition; // Position in Scene World
        gridX = _gridX; // X axis 
        gridY = _gridY; // Y axis.
    }
}
