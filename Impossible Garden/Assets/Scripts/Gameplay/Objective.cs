using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string Title;
    public string Description;

    public bool IsComplete { get; private set; }

    public abstract bool HasDifficulty(DifficultySettings difficulty);
    
    public void CompletionUpdate()
    {
        IsComplete = Evaluate();
    }

    public abstract void Initialize(DifficultySettings difficulty, int numberPlayers);
    protected abstract bool Evaluate(); 
}
