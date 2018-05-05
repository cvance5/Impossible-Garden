using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    public List<Objective> Objectives;
    public List<Objective> ActiveObjectives { get; private set; }

    public T GetActiveObjective<T>() where T : Objective => ActiveObjectives.Find(obj => obj.GetType().IsAssignableFrom(typeof(T))) as T;
    public bool IsUsingObjective<T>() where T : Objective => GetActiveObjective<T>() != null;

    public void PrepareObjectivesForPlayers(List<Player> players)
    {
        ActiveObjectives = new List<Objective>();
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
            ActiveObjectives.Add(objective);
            player.SetObjective(objective);

            availableObjectives[index] = null;
        }
    }

    public List<PlantTypes> GetRequiredPlants()
    {
        List<PlantTypes> requiredPlants = new List<PlantTypes>();

        foreach (Objective objective in ActiveObjectives)
        {
            if (objective is PlantObjective)
            {
                var newRequiredPlants = (objective as PlantObjective).GetRequiredPlants();
                foreach (PlantTypes newPlant in newRequiredPlants)
                {
                    if (!requiredPlants.Contains(newPlant))
                    {
                        requiredPlants.Add(newPlant);
                    }
                }
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

        foreach (Objective objective in ActiveObjectives)
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