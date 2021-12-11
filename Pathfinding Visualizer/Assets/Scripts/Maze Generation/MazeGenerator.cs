using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maze { DFS }
public class MazeGenerator
{
    public void GenerateMaze()
    {

    }

    public IEnumerator DepthFirstGeneration(Node startingNode, Node targetNode, float time)
    {
        if (startingNode == null) yield return default;

        Stack<Node> frontier = new Stack<Node>();
        frontier.Push(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            GameManager.Instance.iterations++;

            var current = frontier.Pop();

            if (current == targetNode)
            {
                List<Node> path = new List<Node>();
                path.Add(current);
                Node next = cameFrom[current];

                while (next != null)
                {
                    path.Add(next);
                    next = cameFrom[next];

                    if (current == targetNode)
                    {
                        path.Reverse();

                        GameManager.Instance.StopLoopAndDrawPath(path, Color.cyan);
                    }
                }
            }

            yield return new WaitForSeconds(time);


            foreach (var item in current.GetAdjacentNeightbours().Where(n => !cameFrom.ContainsKey(n)))
            {
                if (item == startingNode || item == targetNode) continue;


                if (current.isWalkable)
                {

                }
                
                else
                {
                    GameManager.Instance.SetObstacleNode(item);
                    frontier.Push(item);
                    cameFrom.Add(item, current);
                    //AudioManager.Instance.PlayAudio(Utils.Heuristic(item.transform.position, targetNode.transform.position));
                }
            }

        }
    }

    public void BreakWall()
    {

    }

}
