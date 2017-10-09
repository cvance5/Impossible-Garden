using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Plot[,] GardenPlots { get; private set; }

    public void Initialize(Plot[,] plots)
    {
        GardenPlots = plots;

        foreach(Plot plot in plots)
        {
            plot.Initialize();
        }
    }

    public Plot GetPlot(Vector3 location)
    {
        return GetPlot(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.z));
    }
    public Plot GetPlot(int column, int row)
    {
        Plot selectedPlot = null;

        if(column < GardenPlots.GetLength(0) && column >= 0)
        {
            if(row < GardenPlots.GetLength(1) && row >= 0)
            {
                selectedPlot = GardenPlots[column, row];
            }
        }

        return selectedPlot;
    }
}
