using UnityEditor;

public class CustomMenu
{
    [MenuItem("Assets/Create/Settings")]
    public static void CreateGameSettings()
    {
        ScriptableObjectUtility.CreateAsset<GameSettings>();
    }

    [MenuItem("Assets/Create/Seed")]
    public static void CreateSeed()
    {
        ScriptableObjectUtility.CreateAsset<Seed>();
    }

    [MenuItem("Assets/Create/Objectives/Wild Growth Objective")]
    public static void CreateTotalGrowthObjective()
    {
        ScriptableObjectUtility.CreateAsset<WildGrowthObjective>();
    }
    [MenuItem("Assets/Create/Objectives/Walking Path Objective")]
    public static void CreateWalkingPathObjective()
    {
        ScriptableObjectUtility.CreateAsset<WalkingPathObjective>();
    }
    [MenuItem("Assets/Create/Objectives/Predatory Plant Objective")]
    public static void CreatePredatoryPlantObjective()
    {
        ScriptableObjectUtility.CreateAsset<PredatoryPlantObjective>();
    }
    [MenuItem("Assets/Create/Objectives/Zen Balance Objective")]
    public static void CreateZenBalanceObjective()
    {
        ScriptableObjectUtility.CreateAsset<ZenBalanceObjective>();
    }
}
