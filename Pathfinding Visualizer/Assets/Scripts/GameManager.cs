using System;
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
    private MazeGenerator _mazeGenerator;

    public Grid gridScript;

    public Button[] buttons;
    public TMP_Dropdown[] dropdowns;

    public Node startingNode;
    public Node targetNode;
    public Node obstacleNode;

    public bool isRunning;
    public bool canSelect;

    public float debugTime = 1f;
    public int iterations = 0;

    public TextMeshProUGUI iterationDisplay;
    private Algorithm algorithmType;
    public NodesType nodesType;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
            
        else _instance = this;
    }

    private void OnDestroy() 
    { 
        _instance = null; 
    }

    private void Start()
    {
        _pathfinding = new Pathfinding();
        _mazeGenerator = new MazeGenerator();
    }

    public void SetPathfindingAlgorithm(Algorithm index)
    {
        algorithmType = index;
    }

    public void SetMazeAlgorithmAlgorimth(Algorithm index)
    {
        algorithmType = index;
    }


    public void SetNode(NodesType index)
    {
        nodesType = index;
    }
    private void Update()
    {
        iterationDisplay.text = "ITERATIONS:" + iterations.ToString();

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(_mazeGenerator.DepthFirstGeneration(startingNode, targetNode, debugTime));
        }
    }

    public void ResetNodes()
    {
        foreach (Node node in gridScript.ReturnAllGridToList())
        {
            if (node != startingNode && node != targetNode)
            {
                ChangeColor(node.gameObject, Color.white);
                node.isWalkable = true;
            }

            else
            {
                if (startingNode != null || targetNode != null)
                {
                    ChangeColor(startingNode.gameObject, Color.green);
                    ChangeColor(targetNode.gameObject, Color.red);
                }
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

        switch (algorithmType)
        {
            case Algorithm.DepthxFirstxSearch:
                StartCoroutine(_pathfinding.DepthFirstSearch(startingNode, targetNode, debugTime));
                break;
            case Algorithm.BreadxFirstxSearch:
                StartCoroutine(_pathfinding.BreadthFirstSearch(startingNode, targetNode, debugTime));
                break;
            case Algorithm.AxStarxSearch:
                StartCoroutine(_pathfinding.AStar(startingNode, targetNode, debugTime)); 
                break;
            case Algorithm.DijkstraxSearch:
                StartCoroutine(_pathfinding.Dijkstra(startingNode, targetNode, debugTime));
                break;
            case Algorithm.GreedyxBestxFirst:
                StartCoroutine(_pathfinding.GreedyFirstSearch(startingNode, targetNode, debugTime)); 
                break;
        }



        LockUI();
    }
    public void UnlockUI()
    {
        isRunning = false;
        
        foreach (var item in buttons)
        {
            item.interactable = true;
        }
        foreach (var item in dropdowns)
        {
            item.interactable = true;
        }
    }

    public void LockUI()
    {
        isRunning = true;

        foreach (var item in buttons)
        {
            item.interactable = false;
        }
        foreach (var item in dropdowns)
        {
            item.interactable = false;
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
        if (node != startingNode && node != targetNode)
        {
            node.isWalkable = true;
            ChangeColor(node.gameObject, Color.white);
        }

    }
    public void SetObstacleNode(Node node)
    {
        if (node != startingNode && node != targetNode)
        {
            obstacleNode = node;
            node.isWalkable = false;
            ChangeColor(node.gameObject, Color.black);
        }
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
        StartCoroutine(DrawPath(path, color));
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

        UnlockUI();
    }
}
