using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedFeeder
{
    public SmartEvent OnFeed = new SmartEvent();
    public SmartEvent<int> OnSeedRemoved = new SmartEvent<int>();
    public SmartEvent<int> OnSelectionChanged = new SmartEvent<int>();

    public List<Seed> SeedPrototypes { get; private set; }
    public List<Seed> CurrentSeeds { get; private set; }

    public Seed NewestSeed { get; private set; }
    private int _selectedSeed;

    public void Initialize(List<Seed> possibleSeeds)
    {
        SeedPrototypes = possibleSeeds;
        CurrentSeeds = new List<Seed>(NUMBER_SEED_SLOTS);
    }

    public void Feed()
    {
        if (CurrentSeeds.Count == NUMBER_SEED_SLOTS)
        {
            CurrentSeeds.RemoveAt(0);
        }

        while (CurrentSeeds.Count != CurrentSeeds.Capacity)
        {
            Seed randomSeed = SeedFactory.Clone(SeedPrototypes.RandomItem());
            CurrentSeeds.Add(randomSeed);
            NewestSeed = randomSeed;

            OnFeed.Raise();
        }
    }

    public void SetSeedSelection(int selection)
    {
        if(selection >= 0 && selection < CurrentSeeds.Count)
        {
            _selectedSeed = selection;
            OnSelectionChanged.Raise(_selectedSeed);
        }
    }

    public void UseSelectedSeed()
    {
        CurrentSeeds[_selectedSeed] = null;
        OnSeedRemoved.Raise(_selectedSeed);
    }

    public Type GetSelectedSeed()
    {
        Type type = null;

        if (CurrentSeeds[_selectedSeed] != null)
        {
            type = CurrentSeeds[_selectedSeed].PlantType;
        }

        return type;
    }    

    public const int NUMBER_SEED_SLOTS = 5;
}
