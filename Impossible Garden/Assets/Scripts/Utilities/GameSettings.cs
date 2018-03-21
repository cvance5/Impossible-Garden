using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    [Header("TesterSettings")]
    public int NumberPlayers = 1;
    public int NumberTurns = 15;

    [Header("Plant Settings")]
    public List<int> GrassStageDurations;
    public List<int> CloverStageDurations;
    public List<int> FernStageDurations;
    public List<int> MossStageDurations;

    [Header("Game Settings")]
    public DifficultySettings Difficulty;

    [Header("Unity Settings")]
    public Texture2D CurserTexture;

    public Dictionary<Type, List<int>> DurationMap;

    public void OnValidate()
    {
        ConstructDurationMap();

        if(NumberPlayers < 1)
        {
            NumberPlayers = 1;
        }
    }
    private void ConstructDurationMap()
    {
        DurationMap = new Dictionary<Type, List<int>>()
        {
            {typeof(Shimmergrass), GrassStageDurations },
            {typeof(HeartstringClover), CloverStageDurations },
            {typeof(Fern), FernStageDurations },
            {typeof(Moss), MossStageDurations }
        };
    }
}
