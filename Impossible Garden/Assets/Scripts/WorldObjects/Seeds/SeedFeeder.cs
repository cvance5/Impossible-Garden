using System.Collections.Generic;
using UnityEngine;

public class SeedFeeder
{
    public SmartEvent OnFeed = new SmartEvent();

    public List<Seed> SeedPrototypes { get; private set; }
    public List<Seed> CurrentSeeds { get; private set; }

    public Seed NewestSeed { get; private set; }

    public void Initialize(List<Seed> possibleSeeds)
    {
        SeedPrototypes = possibleSeeds;
        CurrentSeeds = new List<Seed>(NUMBER_SEED_SLOTS);        
    }

    public void Feed()
    {
        if(CurrentSeeds.Count == NUMBER_SEED_SLOTS)
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

    public const int NUMBER_SEED_SLOTS = 5;
}
