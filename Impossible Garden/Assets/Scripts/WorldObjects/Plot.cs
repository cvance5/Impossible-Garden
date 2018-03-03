using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerClickHandler
{
    public static SmartEvent<Plot> OnPlotClicked = new SmartEvent<Plot>();

    public PlantActor CurrentPlantActor { get; private set; }

    public Dictionary<Vector3, Plot> Neighbors;

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

    public PlantActor Sow(Type plantType, Player sower = null)
    {
        PlantActor sownPlant = null;

        if (CurrentPlantActor == null)
        {
            Plant newPlant = Activator.CreateInstance(plantType) as Plant;
            newPlant.SetSower(sower);
            CurrentPlantActor = GardenManager.Instance.FillPlot(newPlant, this);
            sownPlant = CurrentPlantActor;
        }
        else
        {
            Log.Info("Plot " + this + " is already occupied by plant " + CurrentPlantActor + " and cannot accept plant " + plantType);
        }

        return sownPlant;
    }

    public bool IsNeighbor(Plot plot)
    {
        bool isNeighbor = false;

        if (Neighbors.ContainsValue(plot))
        {
            isNeighbor = true;
        }

        return isNeighbor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPlotClicked.Raise(this);
    }
}
