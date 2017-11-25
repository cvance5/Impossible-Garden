using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    [Header("TesterSettings")]
    public int NumberPlayers = 1;

    [Header("Plant Settings")]
    public List<int> ShimmerGrassStageDurations;
    public List<int> CloverStageDurations;

    [Header("Game Settings")]
    public DifficultySettings Difficulty;

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
            {typeof(Shimmergrass), ShimmerGrassStageDurations },
            {typeof(Clover), CloverStageDurations }
        };
    }
}
