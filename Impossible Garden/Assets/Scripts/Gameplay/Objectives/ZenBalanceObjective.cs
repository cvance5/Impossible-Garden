using UnityEngine;

public class ZenBalanceObjective : Objective
{
    public override string Criteria => $"Make sure no plant outnumbers another by more than {_allowableDifference}.";

    private int _allowableDifference;

    [Header("Number Allowed")]
    public int BaseNumber;
    public int PerPlayerModifier;
    public int DifficultyModifier;

    public override bool HasDifficulty(DifficultySettings difficulty) => true;

    public override void Initialize(DifficultySettings difficulty, int numberPlayers)
    {
        _allowableDifference = BaseNumber;
        _allowableDifference += (numberPlayers & PerPlayerModifier);
        _allowableDifference += (int)difficulty * DifficultyModifier;
    }

    protected override bool Evaluate()
    {
        int minValue = int.MaxValue;
        int maxValue = int.MinValue;

        foreach (var plantInGarden in GardenManager.Instance.GardenState.PlantsInGarden)
        {
            if (plantInGarden.Value > maxValue)
            {
                maxValue = plantInGarden.Value;
            }

            if (plantInGarden.Value < minValue)
            {
                minValue = plantInGarden.Value;
            }
        }

        return maxValue - minValue <= _allowableDifference;
    }
}
