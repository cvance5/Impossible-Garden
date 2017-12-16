using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> Players { get; private set; }
    public int PlayerCount
    {
        get
        {
            return Players.Count;
        }
    }

    public List<Player> AssignPlayers(List<User> users)
    {
        Players = new List<Player>();

        for (int userNumber = 0; userNumber < users.Count; userNumber++) 
        {
            User user = users[userNumber];

            GameObject player = new GameObject(user.Username);
            player.transform.SetParent(transform);

            Vector2 startDirection = _playerLocations[Mathf.RoundToInt(((float)_playerLocations.Length / users.Count) * userNumber)];
            Vector2 edge = GardenManager.Instance.ActiveGarden.FindEdge(startDirection);

            PlayerController controller = player.AddComponent<PlayerController>();
            
            Players.Add(new Player(user, controller));
        }

        return Players;
    }

    public void SetTurnController(int turn)
    {
        Player turnPlayer = Players[turn % PlayerCount];

        SetControl(turnPlayer);
    }

    public void SetControl(params Player[] controllers)
    {
        foreach(Player player in Players)
        {
            player.SetControl(controllers.Contains(player));     
        }
    }

    private readonly Vector2[] _playerLocations =
    {
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),        
        new Vector2(-1, -1),
        new Vector2(-1, 0),
        new Vector2(-1, 1),
        new Vector2(0, 1)
    };
}
