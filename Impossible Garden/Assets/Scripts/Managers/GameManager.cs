using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public static SmartEvent OnGameLoaded = new SmartEvent();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GardenManager.Instance);

        OnGameLoaded.Raise();
    }
    private void Start()
    {
        TurnManager.Reset();
        GardenManager.Instance.GenerateGarden(10,10);
    }
}
