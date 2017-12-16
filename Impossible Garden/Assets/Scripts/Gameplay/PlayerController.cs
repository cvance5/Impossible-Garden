using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Plant SelectedPlant { get; private set; }
    public bool HasControl = false;

    private void Awake()
    {
        SelectedPlant = new Shimmergrass();
    }

    void Update()
    {
        if(HasControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject clickTarget = GetClickTarget();
                if (clickTarget != null)
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
                    Log.Warning("Clicked nothing!");
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(TurnManager.Instance.AdvanceTurn());
            }
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
