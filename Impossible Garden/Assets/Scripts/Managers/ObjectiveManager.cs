using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    public List<Objective> Objectives;

    private List<Objective> _assignedObjectives;

    public void PrepareObjectivesForPlayers(List<Player> players)
    {
        _assignedObjectives = new List<Objective>();
        Objective[] availableObjectives = Objectives.ToArray();

        if (availableObjectives.Length < players.Count) throw new System.NotSupportedException("Not enough objectives, will loop infinitely.");

        foreach (Player player in players)
        {
            Objective objective = null;
            int index;

            do
            {
                index = Random.Range(0, availableObjectives.Length);
                objective = ValidateObjective(availableObjectives[index]);
            } while (objective == null);

            objective = Instantiate(objective);
            _assignedObjectives.Add(objective);
            player.SetObjective(objective);

            availableObjectives[index] = null;
        }
    }

    public List<PlantTypes> GetRequiredPlants()
    {
        List<PlantTypes> requiredPlants = new List<PlantTypes>();

        foreach (Objective objective in _assignedObjectives)
        {
            if (objective is PlantObjective)
            {
                var newRequiredPlants = (objective as PlantObjective).GetRequiredPlants();
                foreach (PlantTypes newPlant in newRequiredPlants)
                    requiredPlants.Add(newPlant);
            }
        }

        return requiredPlants;
    }

    private Objective ValidateObjective(Objective possibleObjective)
    {
        if (possibleObjective != null)
        {
            if (!possibleObjective.HasDifficulty(GameManager.Instance.Settings.Difficulty))
            {
                possibleObjective = null;
            }
        }
        return possibleObjective;
    }

    public bool CheckObjectiveCompletion()
    {
        bool hasWon = true;

        foreach (Objective objective in _assignedObjectives)
        {
            objective.CompletionUpdate();
            if (!objective.IsComplete)
            {
                hasWon = false;
            }
        }

        return hasWon;
    }
}