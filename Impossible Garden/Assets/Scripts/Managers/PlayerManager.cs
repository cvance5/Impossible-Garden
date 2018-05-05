using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> Players { get; private set; }
    private LocalPlayer[] _localPlayers = null;
    public LocalPlayer[] LocalPlayers
    {
        get
        {
            if (_localPlayers == null)
            {
                var localPlayers = Players.FindAll(player => player is LocalPlayer);
                _localPlayers = new LocalPlayer[localPlayers.Count];

                for (int index = 0; index < localPlayers.Count; index++)
                {
                    _localPlayers[index] = localPlayers[index] as LocalPlayer;
                }
            }

            return _localPlayers;
        }
    }
    public int PlayerCount => Players.Count;

    private int _hotseatIndex;

    public override void Initialize()
    {
        TurnManager.BeginTurn += OnTurnBegin;
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

    private void OnTurnBegin(int turn)
    {
        TurnManager.TurnTimeOut += CommitPlayerTurns;
        SetControl(LocalPlayers);
    }

    public void SetControl(params Player[] activeControllers)
    {
        foreach (Player player in Players)
        {
            player.PrepareForTurn(activeControllers.Contains(player));
        }

        if (activeControllers.Length > 0)
        {
            _hotseatIndex = 0;
            GiveHotseat(_localPlayers[_hotseatIndex]);
        }
    }

    public void PassHotseat(Directions direction)
    {
        if (direction == Directions.Forward)
        {
            GiveHotseat(_localPlayers.LoopedNext(ref _hotseatIndex));
        }
        else
        {
            GiveHotseat(_localPlayers.LoopedPrevious(ref _hotseatIndex));
        }
    }

    private void GiveHotseat(LocalPlayer hotseatPlayer)
    {
        foreach (LocalPlayer player in _localPlayers)
        {
            player.ReleaseHotseat();
        }

        StartCoroutine(hotseatPlayer.SetHotseat());
    }

    public void OnPlayerCommit()
    {
        bool allPlayersCommitted = true;

        foreach (LocalPlayer player in LocalPlayers)
        {
            if (!player.Controller.IsCommitted)
            {
                allPlayersCommitted = false;
                break;
            }
        }

        if (allPlayersCommitted)
        {
            CommitPlayerTurns();
        }
    }

    private void CommitPlayerTurns()
    {
        foreach (LocalPlayer player in LocalPlayers)
        {
            player.Controller.CommitTurn();
        }

        TurnManager.TurnTimeOut -= CommitPlayerTurns;
        SetControl();
        TurnManager.Instance.CompleteTurn();
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
