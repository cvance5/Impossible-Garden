using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnManager
{
    public static SmartEvent BeginTurn = new SmartEvent();

    public static int TurnNumber;

    public static void Reset()
    {
        TurnNumber = 0;
    }

    public static void AdvanceTurn()
    {
        if(DOTween.TotalPlayingTweens() > 0)
        {
            ClearTweenQueue();
        }
        else
        {

            GardenManager.Instance.GrowAllPlants();
            Log.Info("Turn Number: " + TurnNumber);
        }
    }
    
    private static void ClearTweenQueue()
    {
        while(DOTween.TotalPlayingTweens() > 0)
        {
            DOTween.CompleteAll(true);
        }
    }
}
