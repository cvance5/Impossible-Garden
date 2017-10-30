using UnityEngine;

public class ShimmerGrass : Plant
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

    protected override void RandomizePlantAppearance()
    {
        Manager.transform.Rotate(0, Random.Range(0, 270), 0);
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Manager.SmoothlyScalePlant(new Vector3(1, GrowthTimer / (float)StageDuration[GrowthStage], 1));
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

    }
}
