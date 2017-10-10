using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : Singleton<GardenManager>
{
    public Garden ActiveGarden { get; private set; }

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _defaultPlantObject;
    [SerializeField]
    private GameObject _defaultPlotObject;

    private List<PlantManager> _activePlants;
    private List<PlantManager> _newPlants;
    private List<PlantManager> _removedPlants;

    public void GenerateGarden(int plotsWide, int plotsDeep)
    {
        ResetGarden();

        _activePlants = new List<PlantManager>();
        _newPlants = new List<PlantManager>();
        _removedPlants = new List<PlantManager>();

        GameObject newGarden = new GameObject("Garden");
        newGarden.transform.SetParent(transform);
        ActiveGarden = newGarden.AddComponent<Garden>();

        Plot[,] plots = new Plot[plotsWide, plotsDeep];

        for(int row = 0; row < plotsDeep; row++)
        {
            for(int column = 0; column < plotsWide; column++)
            {
                plots[column, row] = Instantiate(_defaultPlotObject, new Vector3(column, 0, row), Quaternion.identity, newGarden.transform).GetComponent<Plot>();
            }
        }

        ActiveGarden.Initialize(plots);
    }
    public void GrowAllPlants()
    {
        foreach (PlantManager activePlant in _activePlants)
        {
            if(activePlant != null)
            {
                activePlant.GrowPlant();
            }
            else
            {
                _activePlants.Remove(activePlant);
                Log.Info("Plant unexpectedly missing!");
            }
        }

        UpdateActivePlants();
    }
    public PlantManager FillPlot(Plant source, Plot caller)
    {
        GameObject plantObject = Instantiate(_defaultPlantObject, caller.transform);
        //Offset object position above plot
        plantObject.transform.position += Vector3.up / 2;

        PlantManager plantManager = plantObject.GetComponent<PlantManager>();
        plantManager.Initialize(source, caller);
        _newPlants.Add(plantManager);

        return plantManager;
    }
    public void RemovePlant(PlantManager caller)
    {
        if(_activePlants.Contains(caller))
        {
            _removedPlants.Add(caller);
        }
        else if(_newPlants.Contains(caller))
        {
            _newPlants.Remove(caller);
        }
        else
        {
            Log.Warning(caller + " has requested removal, but wasn't in any list.");
        }
    }

    private void UpdateActivePlants()
    {
        _activePlants.AddRange(_newPlants);
        foreach(PlantManager plant in _removedPlants)
        {
            _activePlants.Remove(plant);
        }
        _removedPlants.Clear();
        _newPlants.Clear();
    }

    private void ResetGarden()
    {
        if(ActiveGarden != null)
        {
            Destroy(ActiveGarden.gameObject);
            ActiveGarden = null;
        }
    }
}
