using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : Singleton<GardenManager>
{
    public Garden ActiveGarden;

    public void GenerateGarden(int plotsWide, int plotsDeep)
    {
        ResetGarden();

        GameObject newGarden = new GameObject("Garden");
        newGarden.transform.SetParent(transform);
        ActiveGarden = newGarden.AddComponent<Garden>();

        Plot[,] plots = new Plot[plotsWide, plotsDeep];
    }

    private void ResetGarden()
    {
        Destroy(ActiveGarden.gameObject);
        ActiveGarden = null;
    }
}
