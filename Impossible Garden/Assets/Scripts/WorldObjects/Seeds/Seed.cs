using System;
using UnityEngine;

public class Seed : ScriptableObject
{
    public PlantTypes SeedType;
    public Sprite Icon;
    public Type PlantType
    {
        get
        {
            return SeedType.ToType();
        }
    }
}
