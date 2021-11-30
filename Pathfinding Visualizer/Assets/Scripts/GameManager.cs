using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    private Pathfinding _pathfinding;

    public Grid gridScript;

    private Node startingNode;
    private Node targetNode;
    private Node obstacleNode;

    [HideInInspector] public HashSet<Node> ClosedSet = new HashSet<Node>();

    public float debugTime = 1f;
    public int iterations = 0;

    public TextMeshProUGUI iterationDisplay;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI alertDisplay;

    private Algorithm algorithmType;
    public NodesType nodesType;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        else
            _instance = this;
    }

    private void OnDestroy() 
    { 
        _instance = null; 
    }

    private void Start()
    {
        _pathfinding = new Pathfinding();
    }

    public void SetAlgorimth(Algorithm index)
    {
        algorithmType = index;
    }

    public void SetNode(NodesType index)
    {
        nodesType = index;
    }
    private void Update()
    {
        iterationDisplay.text = "ITERATIONS: " + iterations.ToString();
    }

    public void ResetNodes()
    {
        foreach (Node node in gridScript.ReturnAllGridToList())
        {
            if (!node.isWalkable || !node == startingNode || !node == targetNode)
            {
                ChangeColor(node.gameObject, Color.white);
                node.isWalkable = true;
            }
        }
    }
    public void RunSelectedAlgorithm()
    {
        iterations = 0;

        if (startingNode != null && targetNode != null)
        {
            foreach (Node node in gridScript.ReturnAllGridToList())
            {
                node.UpdateNodeCost(targetNode, startingNode);

                if (node.isWalkable && node != startingNode && node != targetNode)
                {
                    ChangeColor(node.gameObject, Color.white);
                }
            }
        }

        else
        {
            if (startingNode == null)
            {
                StartCoroutine(TriggerErrorText("STARTING NODE"));
                return;
            }

            if (targetNode == null)
            {
                StartCoroutine(TriggerErrorText("TARGET NODE"));
                return;
            }
        }

        switch (algorithmType)
        {
            case Algorithm.Depth_First_Search:
                StartCoroutine(_pathfinding.DepthFirstSearch(startingNode, targetNode, debugTime));
                break;
            case Algorithm.Bread_First_Search:
                StartCoroutine(_pathfinding.BreadthFirstSearch(startingNode, targetNode, debugTime));
                break;
            case Algorithm.A_Star:
                StartCoroutine(_pathfinding.AStar(startingNode, targetNode, debugTime)); 
                break;
            case Algorithm.Dijkstra:
                StartCoroutine(_pathfinding.Dijkstra(startingNode, targetNode, debugTime));
                break;
            case Algorithm.Greedy_Best_First:
                StartCoroutine(_pathfinding.GreedyFirstSearch(startingNode, targetNode, debugTime)); 
                break;
        }
    }

    public void SetTargetNode(Node node)
    {
        if (targetNode != null) ChangeColor(targetNode.gameObject, Color.white);
        targetNode = node;
        ChangeColor(node.gameObject, Color.red);
    }

    public void SetWalkableNode(Node node)
    {
        node.isWalkable = true;
        ChangeColor(node.gameObject, Color.white);
    }
    public void SetObstacleNode(Node node)
    {
        obstacleNode = node;
        node.isWalkable = false;
        ClosedSet.Add(node);
        ChangeColor(node.gameObject, Color.black);
    }

    public void SetStartingNode(Node node)
    {
        if (startingNode != null) ChangeColor(startingNode.gameObject, Color.white);
        startingNode = node;
        ChangeColor(node.gameObject, Color.green);
    }
    public void ChangePathColor(List<Node> path, Color color)
    {
        foreach (var item in path)
        {
            ChangeColor(item.gameObject, color);
        }
    }
    
    public void ChangeColor(GameObject go, Color color)
    {
        if (go == null || go.GetComponent<Renderer>() == null) return;
        go.GetComponent<Renderer>().material.color = color;
    }

    public void StopLoopAndDrawPath(List<Node> path, Color color)
    {
        StopAllCoroutines();
        StartCoroutine(DrawPath(path,color));
    }
    public IEnumerator DrawPath(List<Node> path, Color color)
    {
        foreach (var item in gridScript.ReturnAllGridToList())
        {
            
            if(item != targetNode && item !=startingNode && item.isWalkable) 
                ChangeColor(item.gameObject, Color.white);
        }
        
        yield return new WaitForSeconds(1f);

        foreach (var item in path)
        {
            ChangeColor(item.gameObject, color);
        }

        if (startingNode != null || targetNode != null)
        {
            ChangeColor(startingNode.gameObject, Color.green);
            ChangeColor(targetNode.gameObject, Color.red);
        }
    }

    IEnumerator TriggerErrorText(string errorType)
    {
        alertDisplay.gameObject.SetActive(true);
        alertDisplay.CrossFadeAlpha(0f, 4f, false);
        alertDisplay.text = ("ERROR: PLEASE SELECT A " + errorType);

        yield return new WaitForSeconds(4f);

        alertDisplay.gameObject.SetActive(false);
    }

}
