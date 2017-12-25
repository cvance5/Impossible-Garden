using UnityEngine;

public static class SeedFactory
{
    public static Seed Create(PlantTypes typeOfSeed)
    {
        Seed seed = LoadManager.Load<Seed>(typeOfSeed.ToString(), LoadManager.LookupPath(Directories.Seeds));
        seed = Object.Instantiate(seed);
        return seed;
    }

    public static Seed Clone(Seed seed)
    {
        return Object.Instantiate(seed);
    }
}