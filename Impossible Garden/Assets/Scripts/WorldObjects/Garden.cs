﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Plot[,] Plots { get; private set; }

    private int _columns;
    private int _rows;

    public Vector3 Centerpoint => Plots[_columns / 2, _rows / 2].transform.position;

    public void Initialize(Plot[,] plots)
    {
        Plots = plots;

        _columns = plots.GetLength(0);
        _rows = plots.GetLength(1);

        foreach (Plot plot in plots)
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

    public Plant FindNearest(Type plantType, Plot source, out int nearestDistance, bool ignoreSource = true)
    {
        return FindNearest(plant => plantType.IsAssignableFrom(plant.GetType()), source, out nearestDistance, ignoreSource);
    }

    public Plant FindNearest(Func<Plant, bool> condition, Plot source, out int nearestDistance, bool ignoreSource = true)
    {
        Plant nearestPlant = null;
        List<Plot> plotsToSearch = new List<Plot>();

        if (!ignoreSource) plotsToSearch.Add(source);

        foreach (Plot neighbor in source.Neighbors.Values)
            if (neighbor != null) plotsToSearch.Add(neighbor);

        nearestDistance = int.MaxValue;

        for (int currentIndex = 0; currentIndex < plotsToSearch.Count; currentIndex++)
        {
            Plot nextPlot = plotsToSearch[currentIndex];

            foreach (Plot neighbor in nextPlot.Neighbors.Values)
                if (neighbor != null)
                    if (!plotsToSearch.Contains(neighbor)) plotsToSearch.Add(neighbor);

            if (!nextPlot.CurrentPlantActor) continue;
            else if (condition(nextPlot.CurrentPlant))
            {
                nearestDistance = Distance(source, nextPlot);
                nearestPlant = nextPlot.CurrentPlant;
                break;
            }
        }

        return nearestPlant;
    }

    public int Distance(Plot plot1, Plot plot2)
    {
        return (int)(Mathf.Abs(plot1.transform.position.x - plot2.transform.position.x)
                                    + Mathf.Abs(plot1.transform.position.z - plot2.transform.position.z));
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

        if (column < Plots.GetLength(0) && column >= 0)
            if (row < Plots.GetLength(1) && row >= 0)
                selectedPlot = Plots[column, row];

        return selectedPlot;
    }
    public Plot GetPlot(Func<Plot[,], Plot> condition)
    {
        Plot selectedPlot = null;

        selectedPlot = condition(Plots);

        return selectedPlot;
    }
}
