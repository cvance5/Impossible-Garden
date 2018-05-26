using System;
using UnityEngine;

public static class PlantColors
{
    public static readonly Color Shimmergrass = new Color(.1765f, .8353f, .9686f); // 45, 213, 247
    public static readonly Color HeartstringClover = new Color(1, 0, 0); // 255, 0, 0
    public static readonly Color Fern = new Color(0, 1, 0);
    public static readonly Color ShinigaMishk = new Color(.5922f, .9137f, .9843f); // 151, 233, 251

    public static Color ColorByType(PlantTypes plant)
    {
        switch (plant)
        {
            case PlantTypes.Shimmergrass: return Shimmergrass;
            case PlantTypes.HeartstringClover: return HeartstringClover;
            case PlantTypes.Fern: return Fern;
            case PlantTypes.ShinigaMishk: return ShinigaMishk;
            default:
                Log.Warning("No plant color assigned to " + plant);
                return Color.black;
        }
    }
    public static Color ColorByType(Type plant)
    {
        PlantTypes enumType = new PlantTypes();
        enumType = enumType.ToEnum(plant);
        return ColorByType(enumType);
    }
}
