using System;
using System.Collections.Generic;
using UnityEngine;

public class PredatoryPlantObjective : PlantObjective
{
    public Type PlantType;
    public override string Criteria => $"Use {PlantType} to consume {_numberToWin} other plants.";

    [Header("Number Needed")]
    public int BaseNumber;
    public int PerPlayerModifier;

    private int _numberToWin;
    private int _currentTally;

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

        PredatoryTrait.OnKill += OnAnyKill;
    }

    private void OnAnyKill(Plant killer, Plant victim)
    {
        if (killer.GetType() == PlantType)
            _currentTally++;
    }

    protected override bool Evaluate() => _currentTally >= _numberToWin;
}