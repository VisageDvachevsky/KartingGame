using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class GridGraph
{
    private readonly List<Vector2> _neightbourDirections = new List<Vector2>()
    {
        new Vector2( -1, 0 ), // left
        new Vector2( 0, 1 ),  // top
        new Vector2( 1, 0 ),  // right
        new Vector2( 0, -1 ), // bottom
    };

    public int Width;
    public int Height;

    public Node[,] Grid;
    public List<Vector2> Walls;
    public List<Vector2> Forests;


    public GridGraph(int w, int h, bool eightDirectional = true)
    {
        Width = w;
        Height = h;

        Grid = new Node[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Grid[x, y] = new Node(x, y);
            }
        }
    }

    /// <summary>
    /// Checks whether the neighbouring Node is within the grid bounds or not
    /// </summary>
    public bool InBounds(Vector2 v)
    {
        if (v.x >= 0 && v.x < this.Width &&
            v.y >= 0 && v.y < this.Height)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks whether the neighbouring Node is a wall or not
    /// </summary>
    public bool Passable(Vector2 id)
    {
        if (Walls.Contains(id)) return false;
        else return true;
    }

    /// <summary>
    /// Returns a List of neighbouring Nodes
    /// </summary>
    public List<Node> Neighbours(Node n)
    {
        List<Node> results = new List<Node>();

        foreach (Vector2 v in _neightbourDirections)
        {
            Vector2 newVector = v + n.Position;
            if (InBounds(newVector) && Passable(newVector))
            {
                results.Add(Grid[(int)newVector.x, (int)newVector.y]);
            }
        }

        return results;
    }

    public int Cost(Node b)
    {
        // If Node 'b' is a Forest return 2, otherwise 1
        if (Forests.Contains(b.Position)) return 2;
        else return 1;
    }
}
