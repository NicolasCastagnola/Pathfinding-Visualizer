using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maze { KruskalxAlgorithm, PrimsxAlgorithm, RecursivexDivision, RecursivexBacktracking, BinaryxTree }
public class MazeGenerator
{
    public IEnumerator RecursiveBacktracking(List<Node> allNodes, Node startFromHere, Node startingNode, Node targetNode, float time)
    {
        List<Node> unvisted = allNodes;

        List<Node> stack = allNodes;

        Node current = startFromHere;

        Node checkCell = startFromHere;
        if (startFromHere == null) yield return default;


        while (unvisted.Count > 0)
        {
            yield return new WaitForSeconds(time);

            GameManager.Instance.iterations++;

            List<Node> unvisitedNeighbours = current.GetAdjacentNeightbours();

            if (unvisitedNeighbours.Count > 0)
            {
                // Get a random unvisited neighbour.
                checkCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                // Add current cell to stack.
                stack.Add(current);
                // Compare and remove walls.
                CompareWalls(current, checkCell);
                // Make currentCell the neighbour cell.
                current = checkCell;
                // Mark new current cell as visited.
                unvisted.Remove(current);

            }
        }
        //AudioManager.Instance.PlayAudio(Utils.Heuristic(item.transform.position, targetNode.transform.position));
    }


    // Compare neighbour with current and remove appropriate walls.
    public void CompareWalls(Node cCell, Node nCell)
    {
        // If neighbour is left of current.
        if (nCell.thisNodePositionInGrid.x < cCell.thisNodePositionInGrid.x)
        {
            RemoveWall(nCell, 2);
            RemoveWall(cCell, 1);
        }
        // Else if neighbour is right of current.
        else if (nCell.thisNodePositionInGrid.x > cCell.thisNodePositionInGrid.x)
        {
            RemoveWall(nCell, 1);
            RemoveWall(cCell, 2);
        }
        // Else if neighbour is above current.
        else if (nCell.thisNodePositionInGrid.y > cCell.thisNodePositionInGrid.y)
        {
            RemoveWall(nCell, 4);
            RemoveWall(cCell, 3);
        }
        // Else if neighbour is below current.
        else if (nCell.thisNodePositionInGrid.y < cCell.thisNodePositionInGrid.y)
        {
            RemoveWall(nCell, 3);
            RemoveWall(cCell, 4);
        }
    }

    // Function disables wall of your choosing, pass it the script attached to the desired cell
    // and an 'ID', where the ID = the wall. 1 = left, 2 = right, 3 = up, 4 = down.
    public void RemoveWall(Node node, int wallID)
    {
        if (wallID == 1) GameManager.Instance.SetWalkableNode(node.westNode);
        else if (wallID == 2) GameManager.Instance.SetWalkableNode(node.eastNode);
        else if (wallID == 3) GameManager.Instance.SetWalkableNode(node.northNode);
        else if (wallID == 4) GameManager.Instance.SetWalkableNode(node.southNode);
    }

}
