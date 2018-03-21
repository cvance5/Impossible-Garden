using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LocalPlayer Owner { get; private set; }

    public CameraController CameraController;
    public bool HasControl;

    private void Awake()
    {
        Plot.OnPlotClicked += OnPlotSelection;
    }

    private void Update()
    {
        if (HasControl)
        {
            CheckForSeedSelection();

            if (Input.GetKeyDown(KeyCode.Space)) TurnManager.Instance.CompleteTurn();
        }

        if(CameraController != null)
        {
            CameraController.LookUpdate();
        }
    }

    public void AssignOwner(LocalPlayer localPlayer)
    {
        Owner = localPlayer;
    }

    private void OnPlotSelection(Plot clickTarget)
    {
        if (!HasControl) return;

        if (clickTarget != null)
        {
            Plot targetPlot = clickTarget.GetComponent<Plot>();
            if (targetPlot.CurrentPlantActor == null)
            {
                Type selectedPlant = Owner.Feeder.GetSelectedSeed();

                if (targetPlot != null && selectedPlant != null)
                {
                    targetPlot.Sow(selectedPlant, Owner);
                    TurnManager.Instance.CompleteTurn();

                    Owner.Feeder.UseSelectedSeed();
                }
            }
        }
    }

    private void CheckForSeedSelection()
    {
        for (int number = 1; number <= 5; number++)
        {
            if (Input.GetButtonDown("Select" + number))
                Owner.Feeder.SetSeedSelection(number - 1);
        }
    }
}
