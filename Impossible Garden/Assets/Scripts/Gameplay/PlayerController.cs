using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LocalPlayer Owner { get; private set; }
    public bool HasControl;

    void Update()
    {
        if (HasControl)
        {
            CheckForSeedSelection();
            CheckForPlotSelection();

            if(Input.GetKeyDown(KeyCode.Space))
                TurnManager.Instance.CompleteTurn();
        }
    }

    public void AssignOwner(LocalPlayer localPlayer)
    {
        Owner = localPlayer;
    }

    private void CheckForPlotSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickTarget = GetClickTarget();
            if (clickTarget != null)
            {
                Plot targetPlot = clickTarget.GetComponent<Plot>();
                if(targetPlot.CurrentPlantActor == null)
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
    }

    private GameObject GetClickTarget()
    {
        GameObject target = null;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            target = hit.collider.gameObject;

        return target;
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
