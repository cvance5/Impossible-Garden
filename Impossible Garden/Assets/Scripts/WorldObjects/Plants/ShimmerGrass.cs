using UnityEngine;

public class Shimmergrass : Plant
{
    protected override bool CheckPropogation(Plot target)
    {
        bool shouldPropogate = false;

        if(target.CurrentPlantManager == null)
        {
            if(Manager.MyPlot.IsNeighbor(target))
            {
                shouldPropogate = true;
            }
        }

        return shouldPropogate;
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new System.Collections.Generic.Dictionary<string, GameObject>()
        {
            {"Bell", null },
            {"Blades", null },
            {"Cover", null }
        };
    }

    public override void PreparePlantAppearance()
    {
        Manager.SmoothyMovePlant(PartsMap["Cover"].GetComponent<Collider>().RandomPointWithinBounds(), PartsMap["Bell"], 0);
        Manager.SmoothlyScalePlant(Vector3.zero, PartsMap["Cover"], 0);
        Manager.SmoothlyScalePlant(new Vector3(1, 0, 1), PartsMap["Blades"], 0);
        Manager.SmoothlyScalePlant(Vector3.zero, PartsMap["Bell"], 0);
        Manager.SmoothlyRotatePlant(Vector3.up * Random.Range(0, 360), PartsMap["Bell"], 0);
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Manager.SmoothlyScalePlant(Vector3.one, PartsMap["Cover"]);
                break;
            case 1:
                Manager.SmoothlyScalePlant(new Vector3(1, (GrowthTimer  * .5f), 1), PartsMap["Blades"]);
                break;
            case 2:
                Manager.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
                break;
            case 3:
                Manager.Propogate();
                break;
            case 4:
                Manager.SmoothlyColorPlant(Color.black, PartsMap["Bell"]);
                Manager.SmoothlyColorPlant(Color.black, PartsMap["Blades"]);
                break;
            default:
                Log.Error(this + " does not have growth stage " + GrowthStage + " but is trying to grow in that stage.");
                break;
        }
    }

    protected override void ChangeGrowthStage()
    {
        if(GrowthStage == 2)
        {
            Manager.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) / (float)StageDuration[GrowthStage]), PartsMap["Bell"]);
        }
    }
}
