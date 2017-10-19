using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    [Header("Plant Settings")]
    public List<int> ShimmerGrassStageDurations;
    public List<int> CloverStageDurations;

    public Dictionary<Type, List<int>> DurationMap;

    public void OnValidate()
    {
        ConstructDurationMap();
    }

    private void ConstructDurationMap()
    {
        DurationMap = new Dictionary<Type, List<int>>()
        {
            {typeof(ShimmerGrass), ShimmerGrassStageDurations },
            {typeof(Clover), CloverStageDurations }
        };
    }
}
