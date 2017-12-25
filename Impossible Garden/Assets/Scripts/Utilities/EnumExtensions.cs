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
            case PlantTypes.Clover:
                type = typeof(Clover);
                break;
            default:
                throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + plantType.ToString());
        }

        return type;
    }

    public static PlantTypes ToEnum(this PlantTypes variable, Type type)
    {
        PlantTypes plantTypes;

        if (type == typeof(Shimmergrass))
        {
            plantTypes = PlantTypes.Shimmergrass;
        }
        else if (type == typeof(Clover))
        {
            plantTypes = PlantTypes.Clover;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + type.ToString());
        }

        return plantTypes;
    }
}
