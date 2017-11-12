using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectiveManager
{
    private static List<Objective> _assignedObjectives;

    public static void Initialize()
    {
        TurnManager.EndTurn += CheckObjectiveCompletion;
    }

    public static void PrepareObjectivesForPlayers(List<Player> players)
    {
        _assignedObjectives = new List<Objective>();
        Objective[] availableObjectives = GameManager.Instance.Settings.Objectives.ToArray();

        foreach(Player player in players)
        {
            Objective objective = null;
            int index;

            do
            {
                index = Random.Range(0, availableObjectives.Length);
                objective = ValidateObjective(availableObjectives[index]);
            } while (objective == null);

            _assignedObjectives.Add(objective);
            player.SetObjective(objective);

            availableObjectives[index] = null;
        }
    }

    private static Objective ValidateObjective(Objective possibleObjective)
    {
        if(!possibleObjective.HasDifficulty(GameManager.Instance.Settings.Difficulty))
        {
            possibleObjective = null;
        }
        return possibleObjective;
    }

    private static void CheckObjectiveCompletion(int turnNumber)
    {
        bool hasWon = true;

        foreach(Objective objective in _assignedObjectives)
        {
            objective.CompletionUpdate();
            if(!objective.IsComplete)
            {
                hasWon = false;
            }
        }

        if(hasWon)
        {
            GameManager.Instance.EndGame();
        }
    }
}