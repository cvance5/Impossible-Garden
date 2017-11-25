using UnityEditor;

public class CustomMenu
{
    [MenuItem("Assets/Create/Settings")]
    public static void CreateGameSettings()
    {
        ScriptableObjectUtility.CreateAsset<GameSettings>();
    }

    [MenuItem("Assets/Create/Objectives/Total Growth Objective")]
    public static void CreateTotalGrowthObjective()
    {
        ScriptableObjectUtility.CreateAsset<TotalGrowthObjective>();
    }
}
