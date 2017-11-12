using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Clover : Plant
{
    private Plot _growthDirection;
    private int _turnsTillPropogate;

    private Clover source;
    private List<Clover> derivatives;

    protected override bool CheckPropogation(Plot plot)
    {        
        bool shouldPropogate = false;

        if(plot == _growthDirection)
        {
            shouldPropogate = true;
        }

        return shouldPropogate;
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new Dictionary<string, GameObject>()
        {
            {"Clover", null }
        };
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Manager.SmoothlyScalePlant(new Vector3(GrowthTimer / (float)StageDuration[GrowthStage], .333f, GrowthTimer / (float)StageDuration[GrowthStage]));
                break;
            case 1:
                if(_growthDirection == null)
                {
                    DetermineGrowthDirection();

                    if(_growthDirection == null)
                    {
                        GrowthTimer++;
                        DelayPropogation();
                    }
                }
                if(_turnsTillPropogate > 0)
                {
                    _turnsTillPropogate--;
                    if(_turnsTillPropogate == 0)
                    {
                        _growthDirection = null;
                    }
                }
                break;
            case 2:
                Manager.SmoothlyColorPlant(new Color(.8f, .5f, .3f));
                break;
            default:
                Log.Error(this + " does not have growth stage " + GrowthStage + " but is trying to grow in that stage.");
                break;
        }
    }

    protected override void ChangeGrowthStage()
    {
        if(GrowthStage == 1)
        {
            DetermineGrowthDirection();
        }
    }

    private void DelayPropogation()
    {
        _turnsTillPropogate = 4;
    }

    private void DetermineGrowthDirection()
    {
        _growthDirection = GardenManager.Instance.ActiveGarden.GetPlot(SeekNearestPlant);
        if(_growthDirection != null)
        {
            if (_growthDirection.CurrentPlantManager != null)
            {
                _growthDirection.CurrentPlantManager.MyPlant.Wilt();
                _growthDirection = null;
            }
            else
            {
                Manager.Propogate();
                DelayPropogation();
            }
        }
    }

    private Plot SeekNearestPlant(Plot[,] plots)
    {
        int distanceToBest = plots.Length;
        Vector2 myLocation = Manager.MyPlot.transform.position;
        Plot closestOccupiedPlot = null;

        foreach (Plot plot in plots)
        {
            if (plot.CurrentPlantManager != null && plot.CurrentPlantManager.MyPlant.GetType() != typeof(Clover))
            {
                int distanceToTarget = Mathf.RoundToInt(Vector2.Distance(plot.transform.position, myLocation));
                if (distanceToTarget < distanceToBest)
                {
                    closestOccupiedPlot = plot;
                }
            }
        }

        Plot targetPlot = null;

        if (closestOccupiedPlot != null)
        {
            int targetPlotDistance = plots.Length;

            foreach (Plot neighbor in Manager.MyPlot.Neighbors.Values)
            {
                if(neighbor != null && (neighbor.CurrentPlantManager == null || neighbor.CurrentPlantManager.MyPlant.GetType() != typeof(Clover)))
                {
                    int distanceFromNeighbor = Mathf.RoundToInt(Vector2.Distance(neighbor.transform.position, closestOccupiedPlot.transform.position));
                    if (distanceFromNeighbor < targetPlotDistance)
                    {
                        targetPlot = neighbor;
                        targetPlotDistance = distanceFromNeighbor;
                    }
                }
            }
        }

        return targetPlot;
    }
}
