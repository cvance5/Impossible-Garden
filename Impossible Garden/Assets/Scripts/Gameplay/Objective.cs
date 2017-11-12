using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string Title;
    public string Description;

    [Header("Plants Per Difficulty")]
    public List<Type> EasyPlants;
    public List<Type> MediumPlants;
    public List<Type> HardPlants;

    private Dictionary<DifficultySettings, List<Type>> plantsPerDifficulty;

    public bool IsComplete { get; private set; }

    public bool HasDifficulty(DifficultySettings difficulty)
    {
        return plantsPerDifficulty[difficulty] != null;
    }

    void OnValidate()
    {
        plantsPerDifficulty = new Dictionary<DifficultySettings, List<Plant>>()
        {
            {DifficultySettings.Easy, EasyPlants },
            {DifficultySettings.Medium, MediumPlants },
            {DifficultySettings.Hard, HardPlants }
        };
    }
    
    public void CompletionUpdate()
    {
        IsComplete = Evaluate();
    }

    public abstract void Initialize(DifficultySettings difficulty, int numberPlayers);
    protected abstract bool Evaluate(); 
}
