using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensionMethods
{ 
    public static void Reset(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static List<Transform> GetAllChildren(this Transform parent)
    {
        List<Transform> children = new List<Transform>();

        for(int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            children.Add(child);
            children.AddRange(child.GetAllChildren());
        }

        return children;
    }

    public static Vector3 RandomPointWithinBounds(this MeshFilter meshFilter)
    {
        Bounds bounds = meshFilter.mesh.bounds;

        float minX = meshFilter.transform.localScale.x * bounds.size.x * 0.5f;
        float minY = meshFilter.transform.localScale.y * bounds.size.y * 0.5f;
        float minZ = meshFilter.transform.localScale.z * bounds.size.z * 0.5f;

        Vector3 randomPoint = new Vector3(
            Random.Range(minX, -minX),
            Random.Range(minY, -minY),
            Random.Range(minZ, -minZ)
            );

        return randomPoint;
    }
    public static Vector3 RandomPointWithinBounds(this Collider collider)
    {

        Bounds bounds = collider.bounds;

        float minX = collider.transform.localScale.x * bounds.size.x * 0.5f;
        float minY = collider.transform.localScale.y * bounds.size.y * 0.5f;
        float minZ = collider.transform.localScale.z * bounds.size.z * 0.5f;

        Vector3 randomPoint = new Vector3(
            Random.Range(minX, -minX),
            Random.Range(minY, -minY),
            Random.Range(minZ, -minZ)
            );

        return randomPoint;
    }
}
