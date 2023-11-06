using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Source , Destination.
    public Transform seeker, target;
    
    GridCreate grid;

    private void Awake()
    {
        grid = GetComponent<GridCreate>();
    }

    //Update is called once per frame.
    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos) 
    {
        Node startNode = grid.NodeFromWorldPoint(startPos); // Source Node
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // Destination Node

        List<Node> openSet = new List<Node>(); // Open list
        HashSet<Node> closedSet = new HashSet<Node>(); //Closed list
        
        // Add Source Node to the Open list.
        openSet.Add(startNode);

        // while open list is not empty
        while (openSet.Count > 0) 
        {
            // current node is the open list element with lowest cost
            Node currentNode = openSet[0]; 
            
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) 
                {
                    // Add Nodes to the Open List
                    currentNode = openSet[i]; 
                }
            }

            openSet.Remove(currentNode);

            // Add Nodes to the Closed List.
            closedSet.Add(currentNode);

            // Checking If Target Node is Reached.
            if (currentNode == targetNode) 
            {
                // Retrace the Final Path
                RetracePath(startNode, targetNode); 
                return;
            }

            // Get the Neighbour Nodes of the Current Node
            foreach (Node neighbor in grid.GetNeighbors(currentNode)) 
            {
                // Check if it is in the Closed List.
                if (!neighbor.walkable || closedSet.Contains(neighbor)) 
                {
                    continue;
                }

                // F cost of current node to the respective neighbor.
                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                // if neighbour is in closed list
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) 
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    // addneighbour to open list if not .
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        // Hueristic funtion.
        int GetDistance(Node nodeA, Node nodeB) 
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); 
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            //diagonal distance . Greates Absolute value of difference btw x & Y co ordinated of start and end node.
            if (dstX > dstY) 
            {
                return 14 * dstY + 10*(dstX - dstY); 
            }

            return 14 * dstX + 10*(dstY - dstX);
        }
    }

    // Retrace Path for shortest path.
    void RetracePath(Node startNode, Node endNode) 
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) 
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }
}

/******************************* psuedo Code *****************************************/

/*
  function A*(start, goal)
  open_list = set containing start
  closed_list = empty set
  start.g = 0
  start.f = start.g + heuristic(start, goal)
  while open_list is not empty
    current = open_list element with lowest f cost
    if current = goal
      return construct_path(goal) // path found
    remove current from open_list
    add current to closed_list
    for each neighbor in neighbors(current)
      if neighbor not in closed_list
        neighbor.f = neighbor.g + heuristic(neighbor, goal)
        if neighbor is not in open_list
          add neighbor to open_list
        else
          openneighbor = neighbor in open_list
          if neighbor.g < openneighbor.g
            openneighbor.g = neighbor.g
            openneighbor.parent = neighbor.parent
  return false // no path exists

function neighbors(node)
  neighbors = set of valid neighbors to node // check for obstacles here
  for each neighbor in neighbors
    if neighbor is diagonal
      neighbor.g = node.g + diagonal_cost // eg. 1.414 (pythagoras)
    else
      neighbor.g = node.g + normal_cost // eg. 1
    neighbor.parent = node
  return neighbors
function construct_path(node)
  path = set containing node
  while node.parent exists
    node = node.parent
    add node to path
  return path
 */
