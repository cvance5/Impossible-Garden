using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Moss : Plant
{
    public bool IsHeart { get; private set; }

    private State _state;

    private List<Moss> _offspring;
    private List<Plot> _targetPlots;

    protected override bool CheckPropogation(Plot plot)
    {
        bool shouldPropogate = false;

        if (_targetPlots.Contains(plot))
            shouldPropogate = true;

        return shouldPropogate;
    }

    protected override void InitializePlantPartsMap()
    {
        PartsMap = new Dictionary<string, GameObject>
        {
            { "Moss", null }
        };
    }

    public override void InitializeData()
    {
        if (_sower != null) IsHeart = true;
        else IsHeart = false;
    }

    protected override void ApplyGrowthEffects()
    {
        if (_targetPlots?.Count > 0) // if we haven't cleaned up from last turn
        {
            foreach (Plot plot in _targetPlots)
            {
                var newMoss = plot.CurrentPlantActor.MyPlant as Moss;

                if (newMoss != null)
                {
                    _offspring.Add(newMoss);
                }
            }
        }

        switch (GrowthStage)
        {
            case 0:
                Actor.SmoothlyScalePlant(Vector3.one);
                break;
            case 1:
                if (IsHeart)
                {
                    if (_state == State.Expanding)
                    {
                        _targetPlots.Clear();
                        SelectTargetPlots();
                    }

                    if (_state == State.Expanding) // didn't find victims
                    {
                        GardenManager.Instance.PreparePropagation(this);
                    }
                    else
                    {
                        StageDuration[GrowthStage] = 4;

                        if (_state == State.Killing)
                        {
                            foreach (var target in _targetPlots)
                                target.CurrentPlantActor?.MyPlant?.Wilt();

                            _state = State.Replacing;
                        }
                        else if (_state == State.Replacing)
                        {
                            _state = State.Stagnant;
                        }
                        else
                        {
                            GardenManager.Instance.PreparePropagation(this);
                        }
                    }
                }
                else
                {
                    GrowthTimer = 0; // offspring live at their Heart's pace
                }
                break;
            case 2:
                Actor.SmoothlyColorPlant(Color.black);

                if (IsHeart)
                    foreach (Moss offspring in _offspring)
                        offspring.Wilt();
                break;
            default:
                Log.Error(this + " does not have growth stage " + GrowthStage + " but is trying to grow in that stage.");
                break;
        }
    }

    protected override void ChangeGrowthStage()
    {
        switch (GrowthStage)
        {
            case 1:
                _offspring = new List<Moss>
                {
                    this
                };
                _targetPlots = new List<Plot>();
                _state = State.Expanding;
                ApplyGrowthEffects();
                break;
        }
    }

    private void SelectTargetPlots()
    {
        foreach (Moss offspring in _offspring)
        {
            var neighbors = offspring.Actor.MyPlot.Neighbors.Values.ToList();

            foreach (var neighbor in neighbors)
            {
                if (neighbor == null) continue; // skip nonexistant neighbors

                if (_state == State.Expanding) // if still expanding
                {
                    if (neighbor.CurrentPlantActor == null) // and if space is unoccupied
                    {
                        _targetPlots.Add(neighbor); // grow to empty neighbors
                    }
                    else if (!(neighbor.CurrentPlantActor?.MyPlant is Moss)) // if anything other than moss is found, switch to killing
                    {
                        _state = State.Killing;
                        _targetPlots.Clear();
                        _targetPlots.Add(neighbor);
                    }
                }
                else // if not expanding
                {
                    if (neighbor.CurrentPlantActor != null)
                        if (!(neighbor.CurrentPlantActor.MyPlant is Moss)) // if anything other than moss, kill it too
                        {
                            _targetPlots.Add(neighbor);
                        }
                }
            }
        }
    }

    private enum State
    {
        Expanding,
        Killing,
        Replacing,
        Stagnant
    }
}
