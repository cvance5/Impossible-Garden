using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LocalPlayer Owner { get; private set; }
    public bool HasControl;

    private int _selectedSeed;

    void Update()
    {
        if (HasControl)
        {
            CheckForClick();
            CheckForSelectionChange();
        }
    }

    public void AssignOwner(LocalPlayer localPlayer)
    {
        Owner = localPlayer;
    }

    private void CheckForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickTarget = GetClickTarget();
            if (clickTarget != null)
            {
                Plot targetPlot = clickTarget.GetComponent<Plot>();
                Type selectedPlant = Owner.Feeder.GetSeedType(_selectedSeed);

                if (targetPlot != null)
                {
                    if (selectedPlant != null)
                    {
                        targetPlot.Sow(selectedPlant);
                        TurnManager.Instance.CompleteTurn();
                    }
                }
            }
            else
            {
                Log.Warning("Clicked nothing!");
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

    private void CheckForSelectionChange()
    {
        for (int number = 1; number < 10; number++)
        {
            if (Input.GetButtonDown("select" + number))
            {
                _selectedSeed = number;
            }
        }
    }

}
