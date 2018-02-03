﻿using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

    public static SmartEvent OnGameLoaded = new SmartEvent();

    public GameSettings Settings;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GardenManager.Instance);

        if(Settings == null)
        {
            Log.Error("No settings for game.");
        }

        TurnManager.EndTurn += OnTurnEnd;
        TurnManager.BeginTurn += OnTurnBegin;

        OnGameLoaded.Raise();
    }
    private void Start()
    {
        TurnManager.Instance.Initialize();

        ObjectiveManager.Instance.Initialize();
        GardenManager.Instance.GenerateGarden(10,10);
        UIManager.Instance.Initialize();

        SeedManager.Initialize();

        InitializePlayers();
        BeginGame();
    }

    private void InitializePlayers()
    {
        List<User> users = new List<User>();
        for (uint i = 0; i < Settings.NumberPlayers; i++)
        {
            User newUser = new User("Player " + i, i);
            newUser.AssignLocal(true);
            users.Add(newUser);
            
        }
        var players = PlayerManager.Instance.AssignPlayers(users);

        ObjectiveManager.Instance.PrepareObjectivesForPlayers(players);
        foreach (Player player in PlayerManager.Instance.Players)
        {
            Log.Info(player.UserData.Username + " has been assigned " + player.GameObjective.Title + ". \n" + player.GameObjective.Description);
            player.GameObjective.Initialize(Settings.Difficulty, Settings.NumberPlayers);
        }
    }

    private void BeginGame()
    {
        SeedManager.DistributeSeeds();
        PlayerManager.Instance.SetTurnController(0);
    }

    private void OnTurnEnd(int turn)
    {
        PlayerManager.Instance.SetControl();
    }

    private void OnTurnBegin(int turn)
    {
        PlayerManager.Instance.SetTurnController(turn);
    }

    public void EndGame()
    {
        Log.Info("Winners!");
    }
}
