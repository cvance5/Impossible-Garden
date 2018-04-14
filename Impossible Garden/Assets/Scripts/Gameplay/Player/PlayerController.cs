using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LocalPlayer Owner { get; private set; }
    public PlotSelector Selector { get; private set; }

    public CameraController CameraController;

    public bool IsCommitted => _turnProgress == TurnProgress.Committed;

    private Plot _selectedPlot;
    private Type _selectedPlant;

    private TurnProgress _turnProgress;
    private bool _hasControl => _turnProgress != TurnProgress.Completed;

    private void Awake()
    {
        Plot.OnPlotClicked += OnSelectPlot;
        Selector = Instantiate(LoadManager.Load<GameObject>("Selector", Directories.Player).GetComponent<PlotSelector>());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Clear();
            TurnManager.Instance.CompleteTurn();
        }
#endif

        if (_turnProgress < TurnProgress.Committed)
        {
            CheckForSeedSelection();
        }

        if (Input.GetButtonDown("Confirm"))
        {
            if (_turnProgress == TurnProgress.Selected) CommitSelection();
            else if (_turnProgress == TurnProgress.Committed) UncommitSelection();
        }

        if (CameraController != null) // May change at a different rate than control and turn progress
        {
            CameraController.LookUpdate();
        }
    }

    public void AssignOwner(LocalPlayer localPlayer)
    {
        Owner = localPlayer;
        Selector.Initialize(Owner.UserData.Color);
    }

    public void GainControl()
    {
        _turnProgress = TurnProgress.Started;
    }

    public void CommitTurn()
    {
        if (_selectedPlant != null && _selectedPlot != null) Plant();
        Clear();
    }

    private void CheckForSeedSelection()
    {
        for (int number = 1; number <= 5; number++)
        {
            if (Input.GetButtonDown("Select" + number)) Owner.Feeder.SetSeedSelection(number - 1);
        }
    }

    private void OnSelectPlot(Plot clickTarget)
    {
        if (_turnProgress > TurnProgress.Selected) return;

        Plot targetPlot = clickTarget;
        if (targetPlot?.CurrentPlantActor == null)
        {
            _selectedPlot = targetPlot;
            Selector.Select(_selectedPlot);
            _turnProgress = TurnProgress.Selected;
        }
    }

    private void CommitSelection()
    {
        _selectedPlant = Owner.Feeder.GetSelectedSeed();

        if (_selectedPlant != null && _selectedPlot != null)
        {
            _turnProgress = TurnProgress.Committed;
            Selector.ToggleCommit(true);
            PlayerManager.Instance.OnPlayerCommit();
        }
    }

    private void UncommitSelection()
    {
        _turnProgress = TurnProgress.Selected;
        Selector.ToggleCommit(false);
    }

    private void Plant()
    {
        _selectedPlot.Sow(_selectedPlant, Owner);
        Owner.Feeder.UseSelectedSeed();
    }

    private void Clear()
    {
        Selector.Deselect();
        _selectedPlot = null;
        _turnProgress = TurnProgress.Completed;
    }
}

public enum TurnProgress
{
    Started,
    Selected,
    Committed,
    Completed
}
