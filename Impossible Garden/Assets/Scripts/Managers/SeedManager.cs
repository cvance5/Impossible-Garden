using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SeedManager
{
    public static Dictionary<Player, SeedFeeder> Feeders { get; private set; }
    private static Dictionary<PlantTypes, Seed> _seedCatalog;

    public static void Initialize()
    {
        Feeders = new Dictionary<Player, SeedFeeder>();
    }

    public static void RegisterFeeder(Player owner, SeedFeeder feeder)
    {
        Feeders.Add(owner, feeder);
    }

    public static void DistributeSeeds()
    {
        List<PlantTypes> seedTypes = ObjectiveManager.Instance.GetRequiredPlants();

        if(PlayerManager.Instance.PlayerCount > Enum<PlantTypes>.Count)
        {
            throw new ArgumentOutOfRangeException("More players than plant types!");
        }
        else
        {
            while (seedTypes.Count < PlayerManager.Instance.PlayerCount)
            {
                PlantTypes newSeed;

                do
                {
                    newSeed = Enum<PlantTypes>.Random;

                } while (seedTypes.Contains(newSeed));

                seedTypes.Add(newSeed);
            }
        }

        foreach(PlantTypes seedType in seedTypes)
        {
            _seedCatalog.Add(seedType, SeedFactory.Create(seedType));
        }

        Dictionary<Player, List<Seed>> distributionMap = new Dictionary<Player, List<Seed>>();
        int seedsPerPlayer = Mathf.CeilToInt(seedTypes.Count * 75f);

        foreach(Player player in PlayerManager.Instance.Players)
        {
            distributionMap.Add(player, new List<Seed>());

            if(player.GameObjective is PlantObjective)
            {
                var requiredPlants = (player.GameObjective as PlantObjective).GetRequiredPlants();

                foreach(PlantTypes plantType in requiredPlants)
                {
                    distributionMap[player].Add(SeedFactory.Clone(_seedCatalog[plantType]));
                }
            }

            while(distributionMap[player].Count < seedsPerPlayer)
            {
                Seed randomSeed = SeedFactory.Clone(_seedCatalog[seedTypes.RandomItem()]);
                if (!distributionMap[player].Any(seed => randomSeed.SeedType == seed.SeedType))
                {
                    distributionMap[player].Add(randomSeed);
                }
            }
        }

        foreach(Player player in Feeders.Keys)
        {
            Feeders[player].Initialize(distributionMap[player]);
        }
    }
}
