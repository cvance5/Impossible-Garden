using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public static SmartEvent OnGameLoaded = new SmartEvent();
    public static SmartEvent OnGameBegin = new SmartEvent();

    public GameSettings Settings;

    private void Awake()
    {
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Confined;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GardenManager.Instance);

        if (Settings == null)
        {
            Log.Error("No settings for game.");
        }

        TurnManager.EndTurn += OnTurnEnd;

        OnGameLoaded.Raise();
    }
    private void Start()
    {
        ObjectiveManager.Instance.Initialize();
        GardenManager.Instance.GenerateGarden(10, 10);
        PlayerManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        SeedManager.Initialize();

        InitializePlayers();
        InitializeUI();
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
            Log.Info(player.UserData.Username + " has been assigned " + player.Objective.Title + ". \n" + player.Objective.Description);
            player.Objective.Initialize(Settings.Difficulty, Settings.NumberPlayers);
        }
    }

    private void InitializeUI()
    {
        UIManager.Instance.Get<GameplayScreen>();

        EventCollector<Player> readyUpCollector = new EventCollector<Player>(PlayerManager.Instance.PlayerCount, BeginGame);

        foreach (Player player in PlayerManager.Instance.LocalPlayers)
        {
            var popup = UIManager.Instance.Get<ReadyUpPopup>();
            popup.Initialize(player);
            readyUpCollector.AddEvent(popup.OnReady);
        }
    }

    private void BeginGame()
    {
        SeedManager.DistributeSeeds();
        TurnManager.Instance.Initialize(Settings.TurnTimeLimit);
        OnGameBegin.Raise();
    }

    private void OnTurnEnd(int turn)
    {
        bool hasWon = ObjectiveManager.Instance.CheckObjectiveCompletion();
        if (hasWon || turn == Settings.NumberTurns) EndGame(hasWon);
    }

    public void EndGame(bool hasWon)
    {
        if (hasWon) Log.Info("Winners!");
        else Log.Info("Losers!");
    }
}
