﻿using System.Collections.Generic;
using UnityEngine;

public class WalkingPathObjective : Objective
{
    [Header("Number Needed")]
    public int[] SquaresPerDifficulty;
    public float SquaresPerPlayer;

    private int _squaresRequired;

    public override string Criteria => $"Make a clear path of {_squaresRequired} plots.";

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
        _squaresRequired = Mathf.FloorToInt(SquaresPerDifficulty[(int)difficulty] + (numberPlayers * SquaresPerPlayer));
    }

    protected override bool Evaluate()
    {
        bool isComplete = false;

        Plot[,] plots = GardenManager.Instance.ActiveGarden.Plots;

        List<Plot> countedPlots;
        List<Plot> uncheckedPlots;

        foreach(Plot plot in plots)
        {
            if(plot.CurrentPlantActor == null)
            {
                uncheckedPlots = new List<Plot>()
                {
                    plot
                };

                countedPlots = new List<Plot>();

                while(uncheckedPlots.Count >  0)
                {
                    Plot nextPlot = uncheckedPlots[0];
                    uncheckedPlots.Remove(nextPlot);

                    if (nextPlot.CurrentPlantActor != null)
                    {
                        continue;
                    }
                    else if (countedPlots.Contains(nextPlot))
                    {
                        continue;
                    }
                    else
                    {
                        countedPlots.Add(nextPlot);
                        foreach(Plot neighbor in nextPlot.Neighbors.Values)
                        {
                            if(neighbor != null)
                            {
                                if (!uncheckedPlots.Contains(neighbor) && !countedPlots.Contains(neighbor))
                                {
                                    uncheckedPlots.Add(neighbor);
                                }
                            }
                        }
                    }

                    if (countedPlots.Count > _squaresRequired)
                    {
                        isComplete = true;
                        break;
                    }
                }

                if(isComplete)
                {
                    break;
                }
            }            
        }

        return isComplete;

    }
}
