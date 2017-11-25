using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        OnGameLoaded.Raise();
    }
    private void Start()
    {
        TurnManager.Reset();
        PlayerManager.Initialize();

        ObjectiveManager.Instance.Initialize();
        GardenManager.Instance.GenerateGarden(10,10);

        List<User> users = new List<User>();

        for(uint i = 0; i < Settings.NumberPlayers; i++)
        {
            users.Add(new User("Player " + i, i));
        }

        ObjectiveManager.Instance.PrepareObjectivesForPlayers(PlayerManager.AssignPlayers(users));

        foreach(Player player in PlayerManager.Players)
        {
            Log.Info(player.UserData.Username + " has been assigned " + player.GameObjective.Title + ". /n" + player.GameObjective.Description);
            player.GameObjective.Initialize(Settings.Difficulty, Settings.NumberPlayers);
        }
    }


    public void EndGame()
    {

    }
}
