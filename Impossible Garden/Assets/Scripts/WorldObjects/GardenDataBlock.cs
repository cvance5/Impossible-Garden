using System;
using System.Collections.Generic;

public class GardenDataBlock
{
    public Dictionary<Type, int> PlantsInGarden { get; private set; }
    public int LongestPath { get; private set; }

    private static bool _shouldCountPlants;
    private static bool _shouldTrackPaths;
    private static int _maxPathTrackingLength;

    static GardenDataBlock()
    {
        _shouldCountPlants = ObjectiveManager.Instance.IsUsingObjective<WildGrowthObjective>();
        _shouldTrackPaths = ObjectiveManager.Instance.IsUsingObjective<WalkingPathObjective>();

        if (_shouldTrackPaths) _maxPathTrackingLength = ObjectiveManager.Instance.GetActiveObjective<WalkingPathObjective>().SquaresToWin;
    }

    public GardenDataBlock(Garden activeGarden)
    {
        PlantsInGarden = new Dictionary<Type, int>();
        AnalyzePlots(activeGarden.Plots);
    }

    public int GetNumberOfPlants(Type plantType)
    {
        if (PlantsInGarden.ContainsKey(plantType))
            return PlantsInGarden[plantType];
        else
            return 0;
    }

    private void AnalyzePlots(Plot[,] plots)
    {
        foreach (Plot plot in plots)
        {
            if (plot.CurrentPlant != null)
            {
                AnalyzeOccupiedPlot(plot);
            }
            else
            {
                AnalyzeUnoccupiedPlot(plot);
            }
        }
    }

    private void AnalyzeOccupiedPlot(Plot plot)
    {
        if (_shouldCountPlants)
        {
            LogPlant(plot.CurrentPlant.GetType());
        }
    }

    private void AnalyzeUnoccupiedPlot(Plot plot)
    {
        if (_shouldTrackPaths && LongestPath < _maxPathTrackingLength)
        {
            LookForPath(plot);
        }
    }

    private void LookForPath(Plot plot)
    {
        List<Plot> countedPlots = new List<Plot>();
        List<Plot> uncheckedPlots = new List<Plot>()
        {
            plot
        };

        while (uncheckedPlots.Count > 0)
        {
            Plot nextPlot = uncheckedPlots[0];
            uncheckedPlots.Remove(nextPlot);

            if (nextPlot.CurrentPlantActor != null) continue;
            else if (countedPlots.Contains(nextPlot)) continue;
            else
            {
                countedPlots.Add(nextPlot);
                foreach (Plot neighbor in nextPlot.Neighbors.Values)
                {
                    if (neighbor != null)
                    {
                        if (!uncheckedPlots.Contains(neighbor) && !countedPlots.Contains(neighbor))
                        {
                            uncheckedPlots.Add(neighbor);
                        }
                    }
                }
            }

            if (countedPlots.Count >= _maxPathTrackingLength)
            {
                LongestPath = _maxPathTrackingLength;
                return;
            }
        }

        LongestPath = countedPlots.Count;
    }

    private void LogPlant(Type plantType)
    {
        if (PlantsInGarden.ContainsKey(plantType))
            PlantsInGarden[plantType]++;
        else
            PlantsInGarden.Add(plantType, 1);
    }
}
