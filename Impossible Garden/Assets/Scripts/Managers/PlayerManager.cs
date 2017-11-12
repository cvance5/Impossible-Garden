using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
    public static List<Player> Players { get; private set; }
    public static int PlayerCount
    {
        get
        {
            return Players.Count;
        }
    }

    public static void Initialize()
    {
        TurnManager.BeginTurn += GivePlayerControl;
    }

    public static List<Player> AssignPlayers(List<User> users)
    {
        Players = new List<Player>();

        foreach(User user in users)
        {
            Players.Add(new Player(user));
        }

        return Players;
    }

    private static void GivePlayerControl(int turnNumber)
    {

    }
}
