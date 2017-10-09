using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Plant SelectedPlant { get; private set; }

    private void Awake()
    {
        SelectedPlant = new Grass();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject clickTarget = GetClickTarget();
            if(clickTarget != null)
            {
                Plot targetPlot = clickTarget.GetComponent<Plot>();

                if (targetPlot != null)
                {
                    targetPlot.Sow(SelectedPlant);

                    SelectedPlant = new Grass();
                }
            }
            else
            {
                Log.Info("Clicked nothing!");
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TurnManager.AdvanceTurn();
        }
    }
    
    private GameObject GetClickTarget()
    {
        GameObject target = null;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit ))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}
