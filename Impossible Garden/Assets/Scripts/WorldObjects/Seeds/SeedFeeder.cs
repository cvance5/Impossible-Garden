using System.Collections.Generic;
using UnityEngine;

public class SeedFeeder
{
    public List<Seed> SeedsAvailable { get; private set; }
    public List<Seed> CurrentSeeds { get; private set; }

    public void Initialize(List<Seed> possibleSeeds)
    {
        SeedsAvailable = possibleSeeds;
        CurrentSeeds = new List<Seed>(NUMBER_SEED_SLOTS);        
    }

    public void Feed()
    {
        CurrentSeeds.RemoveAt(0);
        Seed randomSeed = SeedFactory.Clone(SeedsAvailable.RandomItem());
        CurrentSeeds.Add(randomSeed);
    }

    private const int NUMBER_SEED_SLOTS = 5;
}
