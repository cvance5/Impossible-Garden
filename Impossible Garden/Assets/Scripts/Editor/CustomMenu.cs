using UnityEditor;

public class CustomMenu
{
    [MenuItem("Assets/Create/Settings")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<GameSettings>();
    }
}
