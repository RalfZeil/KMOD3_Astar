using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path from the startPos to the endPos
    /// Note that you will probably need to add some helper functions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = new Node(startPos, null, 0, 0);
        Node endNode = null;

        //initialize the end and start node
        openList.Add(startNode);

        while(openList.Count != 0)
        {
            Node lowestFNode = null;

            //Get the node with the lowest F value
            foreach (Node node in openList) 
            {
                if(lowestFNode == null) { lowestFNode = node; }
                else if (lowestFNode.FScore > node.FScore) {
                    lowestFNode = node;
                }
            }

            openList.Remove(lowestFNode);
            closedList.Add(lowestFNode);

            List<Node> neighbourNodes = new List<Node>();
            List<Node> newNeighbourNodes = new List<Node>();

            // Make nodes from the given cells
            foreach (Cell cell in GetNeighbouringCells(lowestFNode, grid))
            {
                neighbourNodes.Add(new Node(
                    cell.gridPosition,
                    lowestFNode,
                    lowestFNode.GScore + 1,
                    Vector2Int.Distance(cell.gridPosition, endPos)
                    ));
            }

            // Filter nodes that already exist in open list with lower F value
            foreach (Node node in neighbourNodes)
            {
                foreach(Node openNode in openList)
                {
                    if(node.position == openNode.position)
                    {
                        if(node.FScore > openNode.FScore)
                        {
                            continue;
                        }
                        else
                        {
                            newNeighbourNodes.Add(node); ;
                        }
                    }
                }

                foreach (Node closedNode in closedList)
                {
                    if (node.position == closedNode.position)
                    {
                        if (node.FScore > closedNode.FScore)
                        {
                            continue;
                        }
                        else if (!newNeighbourNodes.Contains(node))
                        {
                            newNeighbourNodes.Add(node);
                        }
                    }
                }

                    openList.Add(node);
                
            }

            // Check each neighbouring Node
            foreach (Node neighbour in newNeighbourNodes)
            {
                neighbour.parent = lowestFNode;

                if (neighbour.position == endPos) 
                {
                    endNode = neighbour;
                    break; 
                }
            }

            // Check if the endnode had been found
            if(endNode != null)
            {
                break;
            }
        } // End While loop

        // Collect the path into a list
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Insert(0, currentNode.position);
            currentNode = currentNode.parent;
        }
        
        return path;
    }

    List<Cell> GetNeighbouringCells(Node node, Cell[,] grid)
    {
        List<Cell> neighbours = new List<Cell>();

        // North
        if (node.position.y + 1 < grid.GetLength(1))
        {
            neighbours.Add(grid[node.position.x, node.position.y + 1]);
        }
        // East
        if (node.position.x + 1 < grid.GetLength(0))
        {
            neighbours.Add(grid[node.position.x + 1, node.position.y]);
        }
        // South
        if (node.position.y - 1 >= 0)
        {
            neighbours.Add(grid[node.position.x, node.position.y - 1]);
        }
        // West
        if (node.position.x - 1 >= 0)
        {
            neighbours.Add(grid[node.position.x - 1, node.position.y]);
        }

        return neighbours;
    }


    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, float GScore, float HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
