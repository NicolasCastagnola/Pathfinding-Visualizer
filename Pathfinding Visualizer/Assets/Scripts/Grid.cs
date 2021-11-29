using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Grid : MonoBehaviour
{
    public GameObject nodePrefab;

    public int gridWidth = 20;
    public int gridHeight = 11;

    private float scale = 1.1f;

    public Node[,] grid;
    private void Start()
    {
        InstantiateGrid();
    }

    public void InstantiateGrid()
    {
        grid = new Node[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var obj = Instantiate(nodePrefab);
                Vector3 tempPos = new Vector3(x * scale, y * scale, 0);
                obj.transform.position = tempPos;
                obj.transform.SetParent(transform);
                obj.GetComponent<Node>().posInGrid = new Vector2Int(x, y);
                obj.GetComponent<Node>().m_NodeGrid = this;
                grid[x, y] = obj.GetComponent<Node>();
                    
            }
        }
    }

    public List<Node> GetNeightbours(int checkX, int checkY)
    {
        List<Node> neightbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 || y == 0)
                {
                    continue;
                }

                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                {
                    neightbours.Add(grid[checkX, checkY]);
                }
            }

        }

        return neightbours;
    }

    public List<Node> ReturnAdjacentsNieghtboursBasedInPostion(int x, int y)
    {
        List<Node> nodes = new List<Node>();
        Vector2Int left = new Vector2Int(x - 1, y);
        Vector2Int right = new Vector2Int(x + 1, y);
        Vector2Int up = new Vector2Int(x, y + 1);
        Vector2Int down = new Vector2Int(x, y - 1);

        if (InBounds(up)) nodes.Add(grid[up.x, up.y]);
        if (InBounds(right)) nodes.Add(grid[right.x, right.y]);
        if (InBounds(down)) nodes.Add(grid[down.x, down.y]);
        if (InBounds(left)) nodes.Add(grid[left.x, left.y]);

        return nodes;
    }

    public List<Node> ReturnNeightboursBasedInCurrentPositionWithDiagonals(int x, int y)
    {
        List<Node> nodes = new List<Node>();

        Vector2Int left = new Vector2Int(x - 1, y);
        Vector2Int right = new Vector2Int(x + 1, y);
        Vector2Int up = new Vector2Int(x, y + 1);
        Vector2Int down = new Vector2Int(x, y - 1);
        Vector2Int leftup = new Vector2Int(x - 1, y + 1);
        Vector2Int rightup = new Vector2Int(x + 1, y + 1);
        Vector2Int leftdown = new Vector2Int(x - 1, y + 1);
        Vector2Int rightdown = new Vector2Int(x + 1, y - 1);


        if (InBounds(up)) nodes.Add(grid[up.x, up.y]);
        if (InBounds(right)) nodes.Add(grid[right.x, right.y]);
        if (InBounds(down)) nodes.Add(grid[down.x, down.y]);
        if (InBounds(left)) nodes.Add(grid[left.x, left.y]);
        if (InBounds(leftup)) nodes.Add(grid[leftup.x, leftup.y]);
        if (InBounds(rightup)) nodes.Add(grid[rightup.x, rightup.y]);
        if (InBounds(leftdown)) nodes.Add(grid[leftdown.x, leftdown.y]);
        if (InBounds(rightdown)) nodes.Add(grid[rightdown.x, rightdown.y]);
        

        return nodes;
    }

    public List<Node> ReturnNeightboursBasedInCurrentPosition(int x, int y)
    {
        List<Node> nodes = new List<Node>();

        Vector2Int up = new Vector2Int(x, y + 1);
        Vector2Int right = new Vector2Int(x + 1, y);
        Vector2Int down = new Vector2Int(x, y - 1);
        Vector2Int left = new Vector2Int(x - 1, y);


        if (InBounds(up)) nodes.Add(grid[up.x, up.y]);
        if (InBounds(right)) nodes.Add(grid[right.x, right.y]);
        if (InBounds(down)) nodes.Add(grid[down.x, down.y]);
        if (InBounds(left)) nodes.Add(grid[left.x, left.y]);

        return nodes;
    }

    public bool InBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x > gridWidth - 1 || pos.y > gridHeight - 1 || pos.y < 0) return false;
    
        return true;
    }

    public void ChangeGridColor(Color color)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].GetComponent<Renderer>().material.color = color;
            }
        }
    }

    public List<Node> ReturnAllGridToList()
    {
        List<Node> nodes = new List<Node>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                nodes.Add(grid[x, y]);
            }
        }

        return nodes;
    }
}
