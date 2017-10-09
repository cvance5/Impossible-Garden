using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public Plant MyPlant { get; private set; }
    public Plot MyPlot { get; private set; }

    private Type _plantType;
    private GameObject _plantVisualizer;

    public void Initialize(Plant newPlant, Plot plot)
    {
        MyPlant = newPlant;
        MyPlant.Manager = this;
        MyPlant.Initialize();
        _plantType = newPlant.GetType();
        _plantVisualizer = Instantiate(LoadManager.LoadResource<GameObject>(_plantType.ToString(), LoadManager.LookupDirectoryPath(Directories.Plants)),transform);

        MyPlant.OnPlantDeath += OnPlantDied;

        MyPlot = plot;
    }
    public void GrowPlant()
    {
        MyPlant.Grow();
    }
    public void Propogate()
    {
        foreach(Plot plot in GardenManager.Instance.ActiveGarden.GardenPlots)
        {
            if(MyPlant.ShouldPropogate(plot))
            {
                plot.Sow(Activator.CreateInstance(_plantType) as Plant);
            }
        }
    }
    public void SmoothlyScalePlant(Vector3 newScale, float duration = 1f)
    {
        _plantVisualizer.transform.DOScale(newScale, duration);
    }
    public void SmoothlyColorPlant(Color newColor, float duration = 1f)
    {
        Material plantMaterial = _plantVisualizer.GetComponent<MeshRenderer>().material;
        plantMaterial.DOColor(newColor, duration);
    }
    private void OnPlantDied()
    {
        Action onPlantRemoved = delegate
        {
            Destroy(this);
        };

                    GardenManager.Instance.RemovePlant(this);
        _plantVisualizer.transform.DOScale(Vector3.zero, 1f).OnComplete(() => onPlantRemoved());
    }
    private void OnDestroy()
    {
        Destroy(_plantVisualizer);
        _plantType = null;
        MyPlant = null;
    }
}
