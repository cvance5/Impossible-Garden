using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : UIScreen
{
    public Transform TurnCounter;
    public Transform TurnMarkerHolder;
    public Transform CounterStart;
    public Transform CounterEnd;

    public Sprite TurnMarkerSprite;

    private List<Transform> _turnMarkers;
    private Tween _moveTween;

    private void Awake()
    {
        TurnManager.BeginTurn += CountTurn;
        _turnMarkers = new List<Transform>();
    }

    public override void ActivateScreen()
    {
        GenerateTurnCounter(GameManager.Instance.Settings.NumberTurns);
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
            turnMarkerTransform.position = Vector3.Lerp(CounterStart.position, CounterEnd.position, delta * turn);
            turnMarkerTransform.localScale = Vector3.one * .15f;
            _turnMarkers.Add(turnMarkerTransform);
        }

        TurnCounter.position = _turnMarkers[0].position;
    }

    private void CountTurn(int turnNumber)
    {
        if (_moveTween != null)
            _moveTween.Complete();

        if (turnNumber < _turnMarkers.Count)
            _moveTween = TurnCounter.DOMove(_turnMarkers[turnNumber].position, 1f);
    }

    private void OnDestroy()
    {
        TurnManager.BeginTurn -= CountTurn;
    }
}
