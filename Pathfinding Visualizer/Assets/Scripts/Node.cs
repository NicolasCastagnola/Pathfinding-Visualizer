using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour
{
    private List<Node> _neighbors = new List<Node>();
    public Vector2Int posInGrid = new Vector2Int();
    public Grid m_NodeGrid;
    public bool isWalkable = true;
    [HideInInspector] public int gCost = 1;
    [HideInInspector] public int hCost = 1;
    [HideInInspector] public int fCost => hCost + gCost;

    [Header("Display")]
    public TextMeshPro gCostDisplay;
    public TextMeshPro hCostDisplay;
    public TextMeshPro fCostDisplay;

    public void UpdateNodeCost(Node targetNode, Node startingNode)
    {
        gCost = Utils.ManhattanDistance(this, targetNode);
        hCost = Utils.ManhattanDistance(this, startingNode);
        gCostDisplay.text = gCost.ToString("0");
        hCostDisplay.text = hCost.ToString("0");
        fCostDisplay.text = fCost.ToString("0");
    }


    public List<Node> GetNeighbours()
    {
        if (_neighbors.Count == 0)
        {
            _neighbors = m_NodeGrid?.ReturnNeightboursBasedInCurrentPosition(posInGrid.x, posInGrid.y);
        }

        return _neighbors;
    }

    public List<Node> GetAdjacentNeightbours()
    {
        if (_neighbors.Count == 0)
        {
            _neighbors = m_NodeGrid?.ReturnAdjacentsNieghtboursBasedInPostion(posInGrid.x, posInGrid.y);
        }

        return _neighbors;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.canSelect)
        {
            if (this == GameManager.Instance.startingNode) 
                GameManager.Instance.nodesType = NodesType.Starting;

            if (this == GameManager.Instance.targetNode) 
                GameManager.Instance.nodesType = NodesType.Target;

            if (this != GameManager.Instance.targetNode && this != GameManager.Instance.startingNode)
                GameManager.Instance.nodesType = isWalkable ?  NodesType.Obstacle : NodesType.Walkable;
        }
    }

    private void OnMouseUp()
    {
        GameManager.Instance.nodesType = NodesType.None;
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.isRunning)
        {
            switch (GameManager.Instance.nodesType)
            {
                case NodesType.Starting:
                    GameManager.Instance.SetStartingNode(this);
                    break;
                case NodesType.Target:
                    GameManager.Instance.SetTargetNode(this);
                    break;
                case NodesType.Obstacle:
                    GameManager.Instance.SetObstacleNode(this);
                    break;
                case NodesType.Walkable:
                    GameManager.Instance.SetWalkableNode(this);
                    break;
            }
        }
    }
}
