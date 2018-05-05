using UnityEngine;

public class WalkingPathObjective : Objective
{
    [Header("Number Needed")]
    public int[] SquaresPerDifficulty;
    public float SquaresPerPlayer;

    public int SquaresToWin { get; private set; }

    public override string Criteria => $"Make a clear path of {SquaresToWin} plots.";

    public override bool HasDifficulty(DifficultySettings difficulty) => true;

    private void OnValidate()
    {
        int numValues = Enum<DifficultySettings>.Count;

        if (SquaresPerDifficulty.Length < numValues)
        {
            SquaresPerDifficulty = new int[numValues];
        }
    }

    public override void Initialize(DifficultySettings difficulty, int numberPlayers)
    {
        SquaresToWin = Mathf.FloorToInt(SquaresPerDifficulty[(int)difficulty] + (numberPlayers * SquaresPerPlayer));
    }

    protected override bool Evaluate() => GardenManager.Instance.GardenState.LongestPath >= SquaresToWin;
}
