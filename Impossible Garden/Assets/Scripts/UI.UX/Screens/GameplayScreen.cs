using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : UIScreen
{
    [Header("Turn Counter")]
    public Transform TurnCounter;
    public Transform TurnMarkerHolder;
    public Transform TurnStart;
    public Transform TurnEnd;

    public Sprite TurnMarkerSprite;

    [Header("Objective Counter")]
    public Transform ObjectiveCore;
    public List<Image> Petals;

    private List<Transform> _turnMarkers;
    private Dictionary<Objective, ColorizerEffect> _pairedRunes;
    private Tween _moveTween;

    private void Awake()
    {
        TurnManager.BeginTurn += AdvanceTurn;
        _turnMarkers = new List<Transform>();
        _pairedRunes = new Dictionary<Objective, ColorizerEffect>();
    }

    public override void ActivateScreen()
    {
        GenerateTurnCounter(GameManager.Instance.Settings.NumberTurns);
        GenerateObjectiveCounter(PlayerManager.Instance.Players);
    }

    private void GenerateTurnCounter(int numTurns)
    {
        float delta = 1f / numTurns;

        if (_turnMarkers.Count > 0)
            foreach (Transform marker in _turnMarkers)
                Destroy(marker.gameObject);

        _turnMarkers.Clear();

        for (int turn = 0; turn < numTurns; turn++)
        {
            GameObject turnMarker = new GameObject("Turn Marker " + turn, typeof(RectTransform), typeof(Image));

            Image turnMarkerImage = turnMarker.GetComponent<Image>();
            turnMarkerImage.sprite = TurnMarkerSprite;

            Transform turnMarkerTransform = turnMarker.GetComponent<Transform>();
            turnMarkerTransform.Reset();
            turnMarkerTransform.SetParent(TurnMarkerHolder);
            turnMarkerTransform.position = Vector3.Lerp(TurnStart.position, TurnEnd.position, delta * turn);
            turnMarkerTransform.localScale = Vector3.one * .15f;
            _turnMarkers.Add(turnMarkerTransform);
        }

        TurnCounter.position = _turnMarkers[0].position;
    }

    private void GenerateObjectiveCounter(List<Player> players)
    {
        foreach (Player player in players)
        {
            var objective = player.Objective;
            int number = player.Number.Value;
            var petal = Petals[number];
            petal.SetActive(true);
            var rune = petal.transform.GetChild(0).GetComponent<Image>();
            rune.sprite = Instantiate(objective.Rune);
            rune.color = Color.white;

            _pairedRunes.Add(objective, rune.GetComponent<ColorizerEffect>());
        }

        UpdateObjectives();
    }

    private void AdvanceTurn(int turnNumber)
    {
        if (_moveTween != null)
            _moveTween.Complete();

        if (turnNumber < _turnMarkers.Count)
            _moveTween = TurnCounter.DOMove(_turnMarkers[turnNumber].position, 1f);

        UpdateObjectives();
    }

    private void UpdateObjectives()
    {
        foreach (Objective objective in _pairedRunes.Keys)
        {
            if (objective.IsComplete) _pairedRunes[objective].ColorizeTo(Color.yellow, 1.5f);
            else _pairedRunes[objective].ColorizeTo(Color.black, 1.5f);
        }
    }

    private void OnDestroy()
    {
        TurnManager.BeginTurn -= AdvanceTurn;
    }
}
