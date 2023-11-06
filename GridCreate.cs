using System.Collections.Generic;
using UnityEngine;

public class GridCreate : MonoBehaviour
{

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;


    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Use this for Initialization.
    private void Start()
    {
        nodeDiameter = nodeRadius * 2; 
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); 
        
        CreateGrid(); 
    }

    // Create 10x10 grid.
    private void CreateGrid() 
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask);
                // Set each node with world value and walkable status.
                grid[x, y] = new Node(walkable, worldPoint, x, y); 
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //Calculating Grid Position from World Position.
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x; 
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y; 
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY); 

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // Return Grid Of Nodes.
        return grid[x, y];
    }

    // Get neighbours
    public List<Node> GetNeighbors(Node node) 
    {
        //List Of Neighbour Nodes.
        List<Node> neighbors = new List<Node>(); 

        for (int x = -1; x <= 1; x++) 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)  
                {
                    // Add neighbours to the grid positions.
                    neighbors.Add(grid[checkX, checkY]); 
                }
            }
        }

        // Returns all the nodes from neighbour list.
        return neighbors;
    }

    public List<Node> path;

    //OnDrawGizmos will use a mouse position that is relative to the Scene View.
    private void OnDrawGizmos() //
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y)); 
        if (grid != null) 
        { 
            foreach (Node n in grid) 
            {
                // Node Colour blue if walkable, Black is N walkable
                Gizmos.color = n.walkable ? Color.blue : Color.black; 
                if (path != null) 
                {
                    if (path.Contains(n)) 
                    {
                        // Shortest Path Colour to White.
                        Gizmos.color = Color.white; 
                    }      
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f)); 
            }
        }
    }
}
