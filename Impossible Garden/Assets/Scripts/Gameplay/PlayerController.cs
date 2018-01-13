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
                Type selectedPlant = Owner.Feeder.GetSelectedSeed();

                if (targetPlot != null)
                {
                    if (selectedPlant != null)
                    {
                        targetPlot.Sow(selectedPlant);
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
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    private void CheckForSeedSelection()
    {        
        for (int number = 1; number <= 5; number++)
        {
            if (Input.GetButtonDown("Select" + number))
            {
                Owner.Feeder.SetSeedSelection(number - 1);
            }
        }        
    }
}
