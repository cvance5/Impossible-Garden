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
            case PlantTypes.Fern:
                type = typeof(Fern);
                break;
            default:
                throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + plantType.ToString());
        }

        return type;
    }

    public static PlantTypes ToEnum(this PlantTypes variable, Type type)
    {
        if (type == typeof(Shimmergrass))
        {
            variable = PlantTypes.Shimmergrass;
        }
        else if (type == typeof(HeartstringClover))
        {
            variable = PlantTypes.HeartstringClover;
        }
        else if (type == typeof(Fern))
        {
            variable = PlantTypes.Fern;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Unknown plant type! Type of " + type.ToString());
        }

        return variable;
    }
}
