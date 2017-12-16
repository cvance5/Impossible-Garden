using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Plot[,] Plots { get; private set; }

    private int _columns;
    private int _rows;

    public Vector3 Centerpoint
    {
        get
        {
            return Plots[_columns / 2, _rows / 2].transform.position;
        }
    }

    public void Initialize(Plot[,] plots)
    {
        Plots = plots;

        _columns = plots.GetLength(0);
        _rows = plots.GetLength(1);

        foreach(Plot plot in plots)
        {
            plot.Initialize();
        }
    }

    public Vector2 FindEdge(Vector2 direction)
    {
        Vector2 edge = new Vector2(_columns / 2, _rows / 2);

        do
        {
            edge += direction;
        } while (GetPlot(edge.x + direction.x, edge.y + direction.y) != null);

        return edge;
    }

    public Plot GetPlot(Vector3 location)
    {
        return GetPlot(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.z));
    }
    public Plot GetPlot(float column, float row)
    {
        return GetPlot((int)column, (int)row);
    }
    public Plot GetPlot(int column, int row)
    {
        Plot selectedPlot = null;

        if(column < Plots.GetLength(0) && column >= 0)
        {
            if(row < Plots.GetLength(1) && row >= 0)
            {
                selectedPlot = Plots[column, row];
            }
        }

        return selectedPlot;
    }
    public Plot GetPlot(Func<Plot[,], Plot> condition)
    {
        Plot selectedPlot = null;

        selectedPlot = condition(Plots);

        return selectedPlot;
    }
}
