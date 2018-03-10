using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string Title;
    public string Description;
    public Sprite Rune;

    public abstract string Criteria { get; }
    
    public bool IsComplete { get; private set; }

    public abstract bool HasDifficulty(DifficultySettings difficulty);
    
    public void CompletionUpdate()
    {
        IsComplete = Evaluate();
    }

    public abstract void Initialize(DifficultySettings difficulty, int numberPlayers);
    protected abstract bool Evaluate(); 
}
