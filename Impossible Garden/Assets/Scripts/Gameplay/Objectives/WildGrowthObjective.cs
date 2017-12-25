using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildGrowthObjective : PlantObjective
{
    public Type PlantType;

    public int NumberToWin;
    public int PerPlayerModifier;

    public override PlantTypes[] GetRequiredPlants()
    {
        PlantTypes requiredPlant = PlantTypes.Shimmergrass;
        requiredPlant = requiredPlant.ToEnum(PlantType);
        return new PlantTypes[] { requiredPlant };
    }

    public override void Initialize(DifficultySettings difficulty, int numberPlayers)
    {
        List<Type> possiblePlantTypes = plantsPerDifficulty[difficulty];
        int selectedIndex = UnityEngine.Random.Range(0, possiblePlantTypes.Count);
        PlantType = possiblePlantTypes[selectedIndex];

        NumberToWin += (PerPlayerModifier * numberPlayers);
    }

    protected override bool Evaluate()
    {
        Plot[,] plots = GardenManager.Instance.ActiveGarden.Plots;

        int amountOfType = 0;
        bool isComplete = false;

        foreach(Plot plot in plots)
        {
            if(plot.CurrentPlantManager != null)
            {
                if(plot.CurrentPlantManager.MyPlant != null)
                {
                    if(plot.CurrentPlantManager.MyPlant.GetType() == PlantType)
                    {
                        amountOfType++;

                        if(amountOfType >= NumberToWin)
                        {
                            isComplete = true;
                            break;
                        }
                    }
                }
            }
        }

        return isComplete;
    }
}
