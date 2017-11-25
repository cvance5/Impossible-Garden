using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string Title;
    public string Description;

    [Header("Plants Per Difficulty")]
    public List<PlantTypes> EasyPlants;
    public List<PlantTypes> MediumPlants;
    public List<PlantTypes> HardPlants;

    protected Dictionary<DifficultySettings, List<Type>> plantsPerDifficulty;

    public bool IsComplete { get; private set; }

    public bool HasDifficulty(DifficultySettings difficulty)
    {
        return plantsPerDifficulty[difficulty] != null;
    }

    void OnValidate()
    {
        plantsPerDifficulty = new Dictionary<DifficultySettings, List<Type>>();

        foreach(DifficultySettings difficulty in Enum.GetValues(typeof(DifficultySettings)))
        {
            plantsPerDifficulty.Add(difficulty, new List<Type>());

            List<PlantTypes> source = null;
            switch(difficulty)
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

            foreach(PlantTypes plantType in source)
            {
                Type type;

                switch(plantType)
                {
                    case PlantTypes.Shimmergrass:
                        type = typeof(Shimmergrass);
                        break;
                    case PlantTypes.Clover:
                        type = typeof(Clover);
                        break;
                    default:
                        Log.Error("Unidentified type!");
                        type = null;
                        break;
                }

                plantsPerDifficulty[difficulty].Add(type);
            }
        }
    }
    
    public void CompletionUpdate()
    {
        IsComplete = Evaluate();
    }

    public abstract void Initialize(DifficultySettings difficulty, int numberPlayers);
    protected abstract bool Evaluate(); 
}
