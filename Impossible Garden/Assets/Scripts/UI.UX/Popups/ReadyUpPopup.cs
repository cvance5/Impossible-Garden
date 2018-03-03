using System;
using UnityEngine.UI;

public class ReadyUpPopup : UIPopup
{
    public SmartEvent<Player> OnReady = new SmartEvent<Player>();

    public Text UserNameLabel;

    public Text ObjectiveTitle;
    public Text ObjectiveDescription;

    public Text NumberOfTurns;
    public Text Difficulty;
    public Text NumberOfObjectives;

    public Button ReadyUpButton;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;

        ShowPlayerName(player.UserData.Username);
        ShowObjective(_player.Objective);
        ShowConditions();
    }

    private void ShowPlayerName(string userName)
    {
        UserNameLabel.text = userName;
    }

    private void ShowObjective(Objective data)
    {
        ObjectiveTitle.text = data.Title;
        ObjectiveDescription.text = data.Description;
    }

    private void ShowConditions()
    {
        NumberOfTurns.text = GameManager.Instance.Settings.NumberTurns.ToString();
        Difficulty.text = GameManager.Instance.Settings.Difficulty.ToString();
        NumberOfObjectives.text = GameManager.Instance.Settings.NumberPlayers.ToString();
    }

    public void OnReadyUpPressed()
    {
        OnReady.Raise(_player);
        ReadyUpButton.interactable = false;
        ReadyUpButton.GetComponentInChildren<Text>().text = "Ready!";

        GameManager.OnGameBegin += OnForceCleanup;
    }

    private void OnForceCleanup()
    {

    }


}
