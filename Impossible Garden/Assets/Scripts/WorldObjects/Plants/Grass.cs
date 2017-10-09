using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{
    protected override void InitializeStageDuration()
    {
        GrowthStage = 0;
        GrowthTimer = 0;
        StageDuration = new Dictionary<int, int>()
        {
            {0, 3},
            {1, 5},
            {2, 1 }
        };
    }
    protected override void InitializePropogationCondition()
    {
        ShouldPropogate = CheckPropogation;
    }
    private bool CheckPropogation(Plot target)
    {
        bool shouldPropogate = false;

        if(target.CurrentPlant == null)
        {
            if(Manager.MyPlot.IsNeighbor(target))
            {
                shouldPropogate = true;
            }
        }

        return shouldPropogate;
    }
    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Manager.SmoothlyScalePlant(new Vector3(.25f, GrowthTimer / (float)StageDuration[GrowthStage], .25f));
                break;
            case 1:
                Manager.Propogate();
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
        Log.Info(this + " has grown to a new stage!");
    }
}
