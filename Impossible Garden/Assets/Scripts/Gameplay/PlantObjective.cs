using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantObjective : Objective {

    [Header("Plants Per Difficulty")]
    public List<PlantTypes> EasyPlants;
    public List<PlantTypes> MediumPlants;
    public List<PlantTypes> HardPlants;

    protected Dictionary<DifficultySettings, List<Type>> plantsPerDifficulty;

    void OnValidate()
    {
        plantsPerDifficulty = new Dictionary<DifficultySettings, List<Type>>();

        foreach (DifficultySettings difficulty in Enum.GetValues(typeof(DifficultySettings)))
        {
            plantsPerDifficulty.Add(difficulty, new List<Type>());

            List<PlantTypes> source = null;
            switch (difficulty)
            {
                case DifficultySettings.Easy:
                    source = EasyPlants;
                    break;
                case DifficultySettings.Medium:
                    source = MediumPlants;
                    break;
                case DifficultySettings.Hard:
                    source = HardPlants;
                    break;
            }

            foreach (PlantTypes plantType in source)
            {
                Type type = plantType.ToType();
                plantsPerDifficulty[difficulty].Add(type);
            }
        }
    }

    public abstract PlantTypes[] GetRequiredPlants();

    public override bool HasDifficulty(DifficultySettings difficulty)
    {
        return plantsPerDifficulty[difficulty] != null;
    }
}
