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

            GameObject playerObject = new GameObject(user.Username);
            playerObject.transform.SetParent(transform);

            int playerNumber = Mathf.RoundToInt(((float)_playerLocations.Length / users.Count) * userNumber);

            Vector2 startDirection = _playerLocations[playerNumber];
            Vector2 edge = GardenManager.Instance.ActiveGarden.FindEdge(startDirection);

            playerObject.transform.position = new Vector3(edge.x, 5, edge.y);

            Player player;

            if (user.IsLocalUser)
            {
                PlayerController controller = playerObject.AddComponent<PlayerController>();
                player = new LocalPlayer(user, controller);
            }
            else player = new Player(user);

            player.SetNumber(playerNumber);
            Players.Add(player);
        }

        return Players;
    }

    public void SetTurnController(int turn)
    {
        Player turnPlayer = Players[turn % PlayerCount];
        SetControl(turnPlayer);
    }

    public void SetControl(params Player[] activeControllers)
    {
        foreach (Player player in Players)
            player.PrepareForTurn(activeControllers.Contains(player));
    }

    private readonly Vector2[] _playerLocations =
    {
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 0),
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(1, 1)
    };
}
