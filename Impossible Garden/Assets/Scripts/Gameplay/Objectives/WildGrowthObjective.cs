﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class WildGrowthObjective : PlantObjective
{
    public Type PlantType;
    public override string Criteria => $"Grow {_numberToWin} {PlantType}!";

    [Header("Number Needed")]
    public int BaseNumber;
    public int PerPlayerModifier;

    private int _numberToWin;

    public override PlantTypes[] GetRequiredPlants()
    {
        PlantTypes requiredPlant = PlantTypes.Shimmergrass; // Assign a default
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

    protected override bool Evaluate() => GardenManager.Instance.GardenState.GetNumberOfPlants(PlantType) >= _numberToWin;
}
