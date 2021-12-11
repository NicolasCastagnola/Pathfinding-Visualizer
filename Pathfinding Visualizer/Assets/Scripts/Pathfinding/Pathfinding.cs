using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    public IEnumerator DepthFirstSearch(Node startingNode, Node targetNode, float time)
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

            current.UpdateNodeCost(targetNode, startingNode);

            if (current == targetNode)
            {
                foreach (var item in cameFrom)
                {
                    item.Key.GetComponent<Renderer>().material.color = Color.white;
                }

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

            current.GetComponent<Renderer>().material.color = Color.green;

            yield return new WaitForSeconds(time);


            foreach (var item in current.GetAdjacentNeightbours().Where(n => !cameFrom.ContainsKey(n)))
            {
                if (!item.isWalkable) continue;

                else
                {
                    frontier.Push(item);
                    cameFrom.Add(item, current);
                    AudioManager.Instance.PlayAudio(Utils.Heuristic(item.transform.position, targetNode.transform.position));
                }
            }

            current.GetComponent<Renderer>().material.color = Color.gray;
        }
    }
    public IEnumerator BreadthFirstSearch(Node startingNode, Node targetNode, float time)
    {
        if (startingNode == null) yield return default;

        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            GameManager.Instance.iterations++;

            var current = frontier.Dequeue();

            current.UpdateNodeCost(targetNode, startingNode);

            if (current == targetNode)
            {
                foreach (var item in cameFrom)
                {
                    item.Key.GetComponent<Renderer>().material.color = Color.white;
                }
                
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

            current.GetComponent<Renderer>().material.color = Color.green;

            yield return new WaitForSeconds(time);


            foreach (var item in current.GetNeighbours())
            {
                if (!item.isWalkable)
                {
                    continue;
                }

                if (!cameFrom.ContainsKey(item))
                {
                    frontier.Enqueue(item);
                    AudioManager.Instance.PlayAudio(Utils.Heuristic(item.transform.position, targetNode.transform.position));
                    cameFrom.Add(item, current);
                    item.GetComponent<Renderer>().material.color = Color.blue;
                }

            }

            current.GetComponent<Renderer>().material.color = Color.gray;
        }

    }
    public IEnumerator GreedyFirstSearch(Node startingNode, Node targetNode, float time)
    {
        if (startingNode == null) yield return default;

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Put(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        yield return new WaitForSeconds(time);

        while (frontier.Count() > 0)
        {
            Node current = frontier.Get();

            current.UpdateNodeCost(targetNode, startingNode);

            yield return new WaitForSeconds(time);

            if (current == targetNode)
            {
                foreach (var item in cameFrom)
                {
                    item.Key.GetComponent<Renderer>().material.color = Color.white;
                }

                current.GetComponent<Renderer>().material.color = Color.white;

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

            foreach (var next in current.GetNeighbours())
            {
                GameManager.Instance.iterations++;

                if (!next.isWalkable) continue;

                foreach (var item in cameFrom.Keys.ToList())
                {
                    item.GetComponent<Renderer>().material.color = Color.gray;
                }

                if (current != targetNode)
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        int priority = Utils.Heuristic(targetNode.transform.position, next.transform.position);
                        AudioManager.Instance.PlayAudio(Utils.Heuristic(next.transform.position, targetNode.transform.position));
                        frontier.Put(next, priority);
                        cameFrom.Add(next, current);
                    }

                    next.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            current.GetComponent<Renderer>().material.color = Color.green;
        }
    }
    public IEnumerator AStar(Node startingNode, Node targetNode, float time)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Put(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count() > 0)
        {
            GameManager.Instance.iterations++;

            Node current = frontier.Get();

            yield return new WaitForSeconds(time);

            if (current == targetNode)
            {
                foreach (var item in cameFrom)
                {
                    item.Key.GetComponent<Renderer>().material.color = Color.white;
                }

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

            foreach (var next in current.GetNeighbours().Where(n => !costSoFar.ContainsKey(n)))
            {
                if (!next.isWalkable) continue;

                int newCost = costSoFar[current] + Utils.ManhattanDistance(current, next);

                if (current != targetNode)
                {

                    foreach (var item in costSoFar)
                    {
                        item.Key.GetComponent<Renderer>().material.color = Color.gray;
                    }

                    foreach (var item in frontier.ReturnDictionaryToList())
                    {
                        item.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }
                
                if (!costSoFar.ContainsKey(next))
                {
                    int priority = newCost + Utils.Heuristic(targetNode.transform.position, next.transform.position);

                    frontier.Put(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);

                    GameManager.Instance.ChangeColor(current.gameObject, Color.green);
                }
                else
                {
                    GameManager.Instance.ChangeColor(current.gameObject, Color.green);

                    if (newCost < costSoFar[next])
                    {
                        int priority = newCost + Utils.Heuristic(targetNode.transform.position, next.transform.position);

                        frontier.Put(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }
                }

                AudioManager.Instance.PlayAudio(Utils.Heuristic(current.transform.position, targetNode.transform.position));
            }
        }
    }
    public IEnumerator Dijkstra(Node startingNode, Node targetNode, float time)
    {
        if (startingNode == null) yield return default;

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Put(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count() > 0)
        {
            Node current = frontier.Get();

            current.UpdateNodeCost(targetNode, startingNode);

            if (current != targetNode)
            {
                current.GetComponent<Renderer>().material.color = Color.green;
            }

            yield return new WaitForSeconds(time);

            if (current == targetNode)
            {
                current.GetComponent<Renderer>().material.color = Color.white;

                foreach (var item in cameFrom)
                {
                    item.Key.GetComponent<Renderer>().material.color = Color.white;
                }

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

            foreach (var next in current.GetNeighbours())
            {
                GameManager.Instance.iterations++;
                 
                if (!next.isWalkable) continue;
                
                int newCost = costSoFar[current] + next.gCost;

                if (current != targetNode)
                {
                    if (!costSoFar.ContainsKey(next) && current != targetNode)
                    {
                        next.GetComponent<Renderer>().material.color = Color.blue;  
                        frontier.Put(next, newCost);
                        cameFrom.Add(next, current);
                        costSoFar.Add(next, newCost);
                    }
                    else
                    {
                        if (newCost < costSoFar[next])
                        {
                            frontier.Put(next, newCost);
                            cameFrom[next] = current;
                            costSoFar[next] = newCost;
                            AudioManager.Instance.PlayAudio(Utils.Heuristic(next.transform.position, targetNode.transform.position));
                        }
                    }
                }
                current.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }
}