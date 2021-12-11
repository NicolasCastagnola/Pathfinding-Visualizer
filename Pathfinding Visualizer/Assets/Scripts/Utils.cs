using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public enum Algorithm { DepthxFirstxSearch, BreadxFirstxSearch, AxStarxSearch, DijkstraxSearch, GreedyxBestxFirst }
public enum NodesType { None, Starting, Target, Obstacle, Walkable}

public class Utils : MonoBehaviour
{
    public static int Heuristic(Vector3 a, Vector3 b)
    {
        return (int)Vector3.Distance(a, b);
    }

    public static int ManhattanDistance(Node current, Node target)
    {
        return Mathf.Abs(current.posInGrid.x - target.posInGrid.x) + Mathf.Abs(current.posInGrid.y - target.posInGrid.y);
    }

    public static bool InSight(Vector3 start, Vector3 end, LayerMask mask)
    {
        Vector3 dir = end - start;
        
        if (!Physics.Raycast(start, dir, dir.magnitude, mask))
        {
            Debug.DrawRay(start, dir, Color.cyan);
            return true;
        }

        else 
            return false;

    }
}
