using System;
using System.Collections.Generic;
using UnityEngine;

public class Fern : Plant
{
    private bool _seedReady;
    private int _seedGrowth;

    private Plot _selectedPlot;

    protected override bool CheckPropogation(Plot plot)
    {
        return plot == _selectedPlot;
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new Dictionary<string, GameObject>()
        {
            {"Fern", null }
        };
    }

    public override void PreparePlantAppearance()
    {
        Actor.SmoothlyScalePlant(Vector3.zero, PartsMap["Fern"], 0);
    }

    protected override void ApplyGrowthEffects()
    {
        switch(GrowthStage)
        {
            case 0:
                Actor.SmoothlyScalePlant(Vector3.one * ((GrowthTimer + 1) * .33f));
                break;
            case 1:
                if (!_seedReady)
                {
                    _seedGrowth++;
                    if (_seedGrowth == 3) _seedReady = true;
                }

                if (_seedReady)
                    if (_selectedPlot != null)
                        GardenManager.Instance.PreparePropagation(this, () =>
                        {
                            _seedReady = false;
                            _seedGrowth = 0;
                            _selectedPlot = null;
                        });
                    else
                        SelectPlot();
                break;
            case 2:
                Actor.SmoothlyColorPlant(Color.black, PartsMap["Fern"]);
                break;
        }
    }

    protected override void ChangeGrowthStage()
    {
        switch(GrowthStage)
        {
            case 1:
                _seedGrowth = 0;
                _seedReady = false;
                break;
        }
    }

    private void SelectPlot()
    {
        Func<Plot[,], Plot> selectPlot = delegate (Plot[,] plots)
        {
            int longestDistance = 0;
            Plot selectedPlot = null;

            foreach (Plot possiblePlot in plots)
            {
                if (possiblePlot.CurrentPlantActor != null) continue;

                int nearestDistance;
                if (GardenManager.Instance.ActiveGarden.FindNearest(typeof(Plant), possiblePlot, out nearestDistance, false) != null)
                    if (nearestDistance > longestDistance)
                    {
                        selectedPlot = possiblePlot;
                        longestDistance = nearestDistance;
                    }
            }

            return selectedPlot;
        };

        _selectedPlot = GardenManager.Instance.ActiveGarden.GetPlot(selectPlot);
    }
}
