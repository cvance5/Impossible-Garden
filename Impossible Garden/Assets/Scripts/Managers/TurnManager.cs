using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnManager
{
    public static SmartEvent<int> BeginTurn = new SmartEvent<int>();
    public static SmartEvent<int> EndTurn = new SmartEvent<int>();

    public static int TurnNumber;

    public static void Reset()
    {
        TurnNumber = 0;
    }

    public static void AdvanceTurn()
    {
        EndTurn.Raise(TurnNumber);

        TurnNumber++;

        if(DOTween.TotalPlayingTweens() > 0)
        {
            ClearTweenQueue();
        }
        else
        {

            GardenManager.Instance.GrowAllPlants();
        }

        BeginTurn.Raise(TurnNumber);
    }
    
    private static void ClearTweenQueue()
    {
        while(DOTween.TotalPlayingTweens() > 0)
        {
            DOTween.CompleteAll(true);
        }
    }
}
