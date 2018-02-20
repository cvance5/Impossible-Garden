using System.Collections.Generic;
using UnityEngine;

public class Moss : Plant
{
    protected override bool CheckPropogation(Plot plot)
    {
        throw new System.NotImplementedException();
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new Dictionary<string, GameObject>
        {
            { "Moss", null }
        };
    }

    protected override void ApplyGrowthEffects()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChangeGrowthStage()
    {
        throw new System.NotImplementedException();
    }
}
