using System.Collections.Generic;
using UnityEngine;

public static class LoadManager
{
    private static Dictionary<Directories, string> _directoryMap = new Dictionary<Directories, string>()
    {
        {Directories.Plants, "PlantPrefabs/" },
        {Directories.Seeds, "Seeds/" }
    };

    public static T Load<T>(string resourceName, string path = "") where T : Object
    {
        var resource = Resources.Load(path + resourceName);

        if (resource == null)
        {
            Log.Error("Failed to load resource at " + path + resourceName + ".");
        }
        if(!(resource is T))
        {
            Log.Error("Resource is not the expected type at " + path + resourceName + ". Result: " + resource.GetType().ToString() + ".");
        }

        return resource as T;
    }

    public static string LookupPath(Directories directory)
    {
        string path;

        if(_directoryMap.ContainsKey(directory))
        {
            path = _directoryMap[directory];
        }
        else
        {
            Log.Error("No directory exists for " + directory);
            path = null;
        }

        return path;
    }
}

public enum Directories
{
    Plants,
    Seeds
}