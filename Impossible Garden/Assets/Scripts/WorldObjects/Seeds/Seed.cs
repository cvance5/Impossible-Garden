using System;
using UnityEngine;

public class Seed : ScriptableObject {

    public PlantTypes SeedType;
    public Sprite Icon;
    private Type _plant;

    private void OnValidate()
    {
        _plant = SeedType.ToType();
    }
    
}
