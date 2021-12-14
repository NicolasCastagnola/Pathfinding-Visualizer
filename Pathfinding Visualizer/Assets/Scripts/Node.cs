using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour
{

    [Header("Grid Stuff")]
    public Vector2Int thisNodePositionInGrid = new Vector2Int();
    public Grid m_NodeGrid;
    private List<Node> _neighborsFromThisNode = new List<Node>();

    [Header("Flags")]
    public bool isWalkable = true;
    public bool isConnected;

    [Header("CurrentNeightBours")]
    public Node northNode;
    public Node westNode;
    public Node southNode;
    public Node eastNode;

    [Header("Cost")]
    public TextMeshPro gCostDisplay;
    public TextMeshPro hCostDisplay;
    public TextMeshPro fCostDisplay;
    [HideInInspector] public int fCost => hCost + gCost;
    [HideInInspector] public int gCost = 1;
    [HideInInspector] public int hCost = 1;


    public void Start()
    {
        northNode = m_NodeGrid?.ReturnNorthNeighbour(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
        westNode = m_NodeGrid?.ReturnWestNeighbour(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
        southNode = m_NodeGrid?.ReturnSouthNeighbour(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
        eastNode = m_NodeGrid?.ReturnEastNeighbour(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
    }

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
        if (_neighborsFromThisNode.Count == 0)
        {
            _neighborsFromThisNode = m_NodeGrid?.ReturnNeightboursBasedInCurrentPosition(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
        }

        return _neighborsFromThisNode;
    }

    public List<Node> GetAdjacentNeightbours()
    {
        if (_neighborsFromThisNode.Count == 0)
        {
            _neighborsFromThisNode = m_NodeGrid?.ReturnAdjacentsNieghtboursBasedInPostion(thisNodePositionInGrid.x, thisNodePositionInGrid.y);
        }

        return _neighborsFromThisNode;
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
        if (!GameManager.Instance.isRunning)
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
