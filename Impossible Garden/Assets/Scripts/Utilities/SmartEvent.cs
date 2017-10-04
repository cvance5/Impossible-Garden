using UnityEngine.Events;

public class SmartEvent : UnityEvent
{
    public delegate void TheDelegate();
    event TheDelegate TheEvent;

    public static SmartEvent operator +(SmartEvent lhs, TheDelegate rhs)
    {
        if (rhs != null)
        {
            lhs.TheEvent += rhs;
        }

        return lhs;
    }

    public static SmartEvent operator -(SmartEvent lhs, TheDelegate rhs)
    {
        if (rhs != null)
        {
            lhs.TheEvent -= rhs;
        }

        return lhs;
    }

    public void Raise()
    {
        if (TheEvent != null)
        {
            TheEvent();
        }
    }
}
