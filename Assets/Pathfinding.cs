using System;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridLength = 5;

    [Tooltip("% Chance to generate an obstacle block in any given grid space.")]
    [SerializeField] private float obstacleChance = 5f;

    private List<Vector2Int> path = new List<Vector2Int>();
    [SerializeField] private Vector2Int start = new Vector2Int(0, 1);
    [SerializeField] private Vector2Int goal = new Vector2Int(4, 4);
    private Vector2Int next;
    private Vector2Int current;

    [HideInInspector] public Vector2Int NewObstacleLocation;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid;

    private void Start()
    {
        GenerateNewGrid();
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        float cellSize = 1f;

        // Draw grid cells
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.color = grid[y, x] == 1 ? Color.black : Color.white;
                Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

        // Draw path
        foreach (var step in path)
        {
            Vector3 cellPosition = new Vector3(step.x * cellSize, 0, step.y * cellSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
        }

        // Draw start and goal
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(start.x * cellSize, 0, start.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(goal.x * cellSize, 0, goal.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private void FindPath(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);
        
        path = new List<Vector2Int>();

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                // print(IsInBounds(next));
                // print(next.y + " " + next.x);

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();
    }

    private void GenerateRandomGrid(int width, int height, float obstacleProbability)
    {
        start.x = Mathf.Clamp(start.x, 0, gridWidth - 1);
        start.y = Mathf.Clamp(start.y, 0, gridLength - 1);

        goal.x = Mathf.Clamp(goal.x, 0, gridWidth - 1);
        goal.y = Mathf.Clamp(goal.y, 0, gridLength - 1);

        grid = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (start.x == i && start.y == j || goal.x == i && goal.y == j)
                {
                    grid[i,j] = 0;
                    continue;
                }

                bool lessThanProbability = UnityEngine.Random.Range(0, 100) < obstacleProbability;
                grid[i, j] = lessThanProbability ? 1 : 0;
            }
        }
    }

    public void AddObstacle(Vector2Int position)
    {
        grid[position.y, position.x] = 1;

        FindPath(start, goal);
    }

    public void GenerateNewGrid()
    {
        GenerateRandomGrid(gridWidth, gridWidth, obstacleChance);
        
        FindPath(start, goal);
    }
}
