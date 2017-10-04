using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    Plot[,] GardenPlots;

    public void Initialize()
    {

    }

    private void OnDestroy()
    {
        foreach(Plot plot in GardenPlots)
        {
            Destroy(plot.gameObject);
        }
    }
}
