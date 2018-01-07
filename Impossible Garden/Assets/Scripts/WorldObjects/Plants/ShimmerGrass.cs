using System.Collections.Generic;
using UnityEngine;

public class Shimmergrass : Plant
{
    private Plot selectedPlot;

    protected override bool CheckPropogation(Plot target)
    {
        bool shouldPropogate = false;

        if (selectedPlot != null) 
        {
            if (target == selectedPlot)
            {
                shouldPropogate = true;
                selectedPlot = null;
            }
        }        

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
        Actor.SmoothyMovePlant(PartsMap["Cover"].GetComponent<Collider>().RandomPointWithinBounds(), PartsMap["Bell"], 0);
        Actor.SmoothlyScalePlant(Vector3.zero, PartsMap["Cover"], 0);
        Actor.SmoothlyScalePlant(new Vector3(1, 0, 1), PartsMap["Blades"], 0);
        Actor.SmoothlyScalePlant(Vector3.zero, PartsMap["Bell"], 0);
        Actor.SmoothlyRotatePlant(Vector3.up * Random.Range(0, 360), PartsMap["Bell"], 0);
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Actor.SmoothlyScalePlant(Vector3.one, PartsMap["Cover"]);
                break;
            case 1:
                Actor.SmoothlyScalePlant(new Vector3(1, (GrowthTimer  * .5f), 1), PartsMap["Blades"]);
                break;
            case 2:
                Actor.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
                break;
            case 3:
                SelectPropogationTarget();
                Actor.Propogate();
                break;
            case 4:
                Actor.SmoothlyColorPlant(Color.black, PartsMap["Bell"]);
                Actor.SmoothlyColorPlant(Color.black, PartsMap["Blades"]);
                break;
            default:
                Log.Error(this + " does not have growth stage " + GrowthStage + " but is trying to grow in that stage.");
                break;
        }
    }

    private void SelectPropogationTarget()
    {
        List<Plot> availableNeighbors = new List<Plot>();

        foreach(Plot neighbor in Actor.MyPlot.Neighbors.Values)
        {
            if(neighbor != null)
            {
                if(neighbor.CurrentPlantActor == null)
                {
                    availableNeighbors.Add(neighbor);
                }
            }
        }
        
        if(availableNeighbors.Count > 0)
        {
            selectedPlot = availableNeighbors.RandomItem();
        }
    }

    protected override void ChangeGrowthStage()
    {
        if(GrowthStage == 2)
        {
            Actor.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
        }
    }
}
