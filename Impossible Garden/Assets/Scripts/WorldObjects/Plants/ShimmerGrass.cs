using System;
using System.Collections.Generic;
using UnityEngine;

public class Shimmergrass : Plant
{
    public bool IsHeart { get; private set; }

    private List<KeyValuePair<Plot, List<PlantActor>>> _reachablePlots;
    private Plot _selectedPlot;    

    protected override bool CheckPropogation(Plot target)
    {
        bool shouldPropogate = false;
        if (target == _selectedPlot) shouldPropogate = true;
        return shouldPropogate;
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new Dictionary<string, GameObject>()
        {
            {"Bell", null },
            {"Blades", null },
            {"Cover", null }
        };
    }

    public override void PreparePlantAppearance()
    {
        Actor.SmoothlyMovePlant(PartsMap["Cover"].GetComponent<Collider>().RandomPointWithinBounds(), PartsMap["Bell"], 0);
        Actor.SmoothlyScalePlant(Vector3.zero, PartsMap["Cover"], 0);
        Actor.SmoothlyScalePlant(new Vector3(1, 0, 1), PartsMap["Blades"], 0);
        Actor.SmoothlyScalePlant(Vector3.zero, PartsMap["Bell"], 0);
        Actor.SmoothlyRotatePlant(Vector3.up * UnityEngine.Random.Range(0, 360), PartsMap["Bell"], 0);
    }

    public override void InitializeData()
    {
        IsHeart = true;

        if(_sower == null)
        {
            Func<Plant, bool> findHeart = delegate (Plant plant)
            {
                if (plant is Shimmergrass)
                    if ((plant as Shimmergrass).IsHeart)
                        return true;

                return false;
            };

            int nearestDistance = -1;

            if (GardenManager.Instance.ActiveGarden.FindNearest(findHeart, Actor.MyPlot, out nearestDistance) != null)
                IsHeart = nearestDistance > DISTANCE_TO_NEAREST_THRESHOLD;
        }

        if (IsHeart)
        {
            _reachablePlots = new List<KeyValuePair<Plot, List<PlantActor>>>();
            FindReachablePlots(Actor.MyPlot, Actor);
        }
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Actor.SmoothlyScalePlant(Vector3.one * .1f, PartsMap["Cover"]);
                break;
            case 1:
                Actor.SmoothlyScalePlant(new Vector3(1, (GrowthTimer  * .5f), 1), PartsMap["Blades"]);
                break;
            case 2:
                if (IsHeart) Actor.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
                else GrowthStage++;
                break;
            case 3:
                if (IsHeart) SeekSpreadTarget();
                else
                {
                    if (!CheckSafe(Actor.MyPlot)) GrowthStage++;
                    else GrowthTimer--;
                }
                break;
            case 4:
                if (IsHeart) Actor.SmoothlyColorPlant(Color.black, PartsMap["Bell"]);
                Actor.SmoothlyColorPlant(Color.black, PartsMap["Blades"]);
                break;
            default:
                Log.Error(this + " does not have growth stage " + GrowthStage + " but is trying to grow in that stage.");
                break;
        }
    }

    private bool CheckSafe(Plot target)
    {
        bool isSafe = true;

        if(target.CurrentPlantActor != null)
            if(target.CurrentPlantActor.MyPlant != this)
                isSafe = false; // Occupied.        
        else
            foreach (Plot targetNeighbor in target.Neighbors.Values)
            {
                if (targetNeighbor == null) continue; // Safe to grow by edge of map
                else if (targetNeighbor == Actor.MyPlot) continue; // Safe to grow by self
                else if (targetNeighbor.CurrentPlantActor != null) // Check type before growing beside anything else...
                {
                    if (targetNeighbor.CurrentPlantActor.MyPlant.GetType() == typeof(Shimmergrass)) continue; // ...and consider only shimmergrass a safe neighbor.
                    else
                    {
                        isSafe = false;
                        break;
                    }
                }
            }

        return isSafe;
    }

    private void SeekSpreadTarget()
    {
        _selectedPlot = null;

        while(_reachablePlots.Count > 0 && _selectedPlot == null)
        {
            Plot testingPlot = _reachablePlots[0].Key;

            bool isStillConnected = true;

            foreach(PlantActor offshoot in _reachablePlots[0].Value)
            {
                if(offshoot == null)
                {
                    isStillConnected = false;
                    break;
                }
                else if(offshoot.MyPlant.GrowthStage == 4)
                {
                    isStillConnected = false; // Shimmergrass is dying
                    break;
                }
            }

            if (isStillConnected)
                if (CheckSafe(testingPlot))
                    _selectedPlot = testingPlot;

            _reachablePlots.RemoveAt(0);
        }

        if(_selectedPlot != null)
        {
            GrowthTimer--;
            Actor.Propogate();

            PlantActor offshoot = _selectedPlot.CurrentPlantActor;

            if(!(offshoot.MyPlant as Shimmergrass).IsHeart)
                FindReachablePlots(_selectedPlot, offshoot);
        }
    }

    private void FindReachablePlots(Plot sourcePlot, PlantActor reachableBy)
    {
        foreach (Plot possiblePlot in sourcePlot.Neighbors.Values)
        {
            if (possiblePlot == null) continue; // Walls don't matter
            else if (possiblePlot.CurrentPlantActor == null)
            {

                bool isFound = false;
                foreach(KeyValuePair<Plot, List<PlantActor>> reachablePlot in _reachablePlots)
                {
                    if(reachablePlot.Key == sourcePlot)
                    {
                        reachablePlot.Value.Add(reachableBy);
                        isFound = true;
                        break;
                    }
                }

                if(!isFound)
                {
                    _reachablePlots.Add(new KeyValuePair<Plot, List<PlantActor>>(possiblePlot, new List<PlantActor>() { reachableBy }));
                }
            }
        }
    }

    protected override void ChangeGrowthStage()
    {
        if(GrowthStage == 2)
        {
            if(IsHeart)
                Actor.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
        }
    }

    private const int DISTANCE_TO_NEAREST_THRESHOLD = 3;
}