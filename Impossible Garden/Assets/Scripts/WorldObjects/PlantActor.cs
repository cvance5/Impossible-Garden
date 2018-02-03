using DG.Tweening;
using System;
using UnityEngine;

public class PlantActor : MonoBehaviour
{
    public Plant MyPlant { get; private set; }
    public Plot MyPlot { get; private set; }

    private Type _plantType;
    private GameObject _plantVisualizer;

    public void Initialize(Plant newPlant, Plot plot)
    {
        MyPlant = newPlant;
        MyPlot = plot;
        MyPlant.Actor = this;
        MyPlant.Initialize();
        _plantType = newPlant.GetType();
        _plantVisualizer = Instantiate(LoadManager.Load<GameObject>(_plantType.ToString(), LoadManager.Path(Directories.Plants)), transform);

        foreach (Transform part in transform.GetAllChildren())
        {
            if (MyPlant.PartsMap.ContainsKey(part.name))
            {
                MyPlant.PartsMap[part.name] = part.gameObject;
            }
        }

        MyPlant.PreparePlantAppearance();

        MyPlant.OnPlantDeath += OnPlantDied;        
    }

    public void GrowPlant()
    {
        MyPlant.Grow();
    }

    public void Propogate()
    {
        foreach (Plot plot in GardenManager.Instance.ActiveGarden.Plots)
        {
            if (MyPlant.ShouldPropogate(plot))
            {
                plot.Sow(_plantType);
            }
        }
    }

    public void SmoothlyMovePlant(Vector3 newLocation, GameObject target = null, float duration = 1f)
    {
        if (target == null)
        {
            target = _plantVisualizer;
        }

        CheckActive(target);

        if (duration == 0)
        {
            target.transform.localPosition = newLocation;
        }
        else
        {
            target.transform.DOLocalMove(newLocation, duration);
        }
    }

    public void SmoothlyRotatePlant(Vector3 additionalRotatation, GameObject target = null, float duration = 1f)
    {
        if (target == null)
        {
            target = _plantVisualizer;
        }

        CheckActive(target);

        if (duration == 0)
        {
            target.transform.localEulerAngles = target.transform.localEulerAngles + additionalRotatation;
        }
        else
        {
            target.transform.DOLocalRotate(target.transform.localEulerAngles + additionalRotatation, duration);
        }
    }

    public void SmoothlyScalePlant(Vector3 newScale, GameObject target = null, float duration = 1f)
    {
        if (target == null)
        {
            target = _plantVisualizer;
        }

        CheckActive(target);

        if (duration == 0)
        {
            target.transform.localScale = newScale;
        }
        else
        {
            target.transform.DOScale(newScale, duration);
        }
    }

    public void SmoothlyColorPlant(Color newColor, GameObject target = null, float duration = 1f)
    {
        if (target == null)
        {
            target = _plantVisualizer;
        }

        CheckActive(target);

        Renderer renderer = target.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = target.GetComponent<ParticleSystemRenderer>();
        }

        Material plantMaterial = renderer.material;

        if (duration == 0)
        {
            plantMaterial.color = newColor;
        }
        else
        {
            plantMaterial.DOColor(newColor, duration);
        }
    }

    public void SetPlantVisibility(bool isVisible, GameObject target = null)
    {
        if (target == null)
        {
            target = _plantVisualizer;
        }

        target.SetActive(isVisible);
    }

    private void CheckActive(GameObject target)
    {
        if (!target.activeInHierarchy)
        {
            target.SetActive(true);
        }
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
