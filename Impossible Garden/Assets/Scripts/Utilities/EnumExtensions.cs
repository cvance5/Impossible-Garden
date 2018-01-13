using System;

public static class EnumExtensions
{
	public static Type ToType(this PlantTypes plantType)
    {
        Type type;

        switch (plantType)
        {
            case PlantTypes.Shimmergrass:
                type = typeof(Shimmergrass);
                break;
            case PlantTypes.HeartstringClover:
                type = typeof(HeartstringClover);
                break;
            default:
                throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + plantType.ToString());
        }

        return type;
    }

    public static void ToEnum(this PlantTypes variable, Type type)
    {
        PlantTypes plantTypes;

        if (type == typeof(Shimmergrass))
        {
            plantTypes = PlantTypes.Shimmergrass;
        }
        else if (type == typeof(HeartstringClover))
        {
            plantTypes = PlantTypes.HeartstringClover;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + type.ToString());
        }

        variable = plantTypes;
    }
}
