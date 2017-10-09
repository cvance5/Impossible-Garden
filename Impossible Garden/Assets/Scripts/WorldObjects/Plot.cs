﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public PlantManager CurrentPlant { get; private set; }

    private Dictionary<Vector3, Plot> Neighbors;

    public void Initialize()
    {
        Neighbors = new Dictionary<Vector3, Plot>
        {
            { Vector3.left, GardenManager.Instance.ActiveGarden.GetPlot(transform.position + Vector3.left) },
            { Vector3.back, GardenManager.Instance.ActiveGarden.GetPlot(transform.position + Vector3.back) },
            { Vector3.right, GardenManager.Instance.ActiveGarden.GetPlot(transform.position + Vector3.right) },
            { Vector3.forward, GardenManager.Instance.ActiveGarden.GetPlot(transform.position + Vector3.forward) }
        };
    }

    public void Sow(Plant newPlant)
    {
        if(CurrentPlant == null)
        {
            CurrentPlant = GardenManager.Instance.FillPlot(newPlant, this);
        }
        else
        {
            Log.Info("Plot " + this + " is already occupied by plant " + CurrentPlant + " and cannot accept plant " + newPlant);
        }
    }

    public bool IsNeighbor(Plot plot)
    {
        bool isNeighbor = false;

        if(Neighbors.ContainsValue(plot))
        {
            isNeighbor = true;
        }

        return isNeighbor;
    }
}
