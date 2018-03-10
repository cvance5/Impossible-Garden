using System;
using System.Collections.Generic;
using UnityEngine;

public class WildGrowthObjective : PlantObjective
{
    public Type PlantType;
    public override string Criteria => $"Grow {_numberToWin} {PlantType}";

    [Header("Number Needed")]
    public int BaseNumber;
    public int PerPlayerModifier;

    private int _numberToWin;

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

        _numberToWin = BaseNumber + (PerPlayerModifier * numberPlayers);
    }

    protected override bool Evaluate()
    {
        Plot[,] plots = GardenManager.Instance.ActiveGarden.Plots;

        int amountOfType = 0;
        bool isComplete = false;

        foreach(Plot plot in plots)
        {
            if(plot.CurrentPlantActor != null)
            {
                if(plot.CurrentPlantActor.MyPlant != null)
                {
                    if(plot.CurrentPlantActor.MyPlant.GetType() == PlantType)
                    {
                        amountOfType++;

                        if(amountOfType >= _numberToWin)
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
