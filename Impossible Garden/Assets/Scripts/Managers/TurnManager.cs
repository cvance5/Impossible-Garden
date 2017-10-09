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
        DOTween.CompleteAll(true);
        GardenManager.Instance.GrowAllPlants();
        Log.Info("Turn Number: " + TurnNumber);
    }
}
