using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Plant SelectedPlant { get; private set; }

    private void Awake()
    {
        SelectedPlant = new Shimmergrass();
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

                    SelectedPlant = new Shimmergrass();
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


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedPlant = new Shimmergrass();
            Log.Info("Selected ShimmerGrass");
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedPlant = new Clover();
            Log.Info("Selected Clover");
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
