using System.Collections.Generic;

public static class List
{
    public static T RandomItem<T>(this List<T> list)
    {
        int randomValue = UnityEngine.Random.Range(0, list.Count);
        return list[randomValue];
    }
}
