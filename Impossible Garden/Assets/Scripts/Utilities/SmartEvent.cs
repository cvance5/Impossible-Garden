﻿using UnityEngine.Events;

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
        TheEvent?.Invoke();
    }
}

public class SmartEvent<T> : UnityEvent
{
    public delegate void TheDelegate(T arg);
    event TheDelegate TheEvent;

    public static SmartEvent<T> operator +(SmartEvent<T> lhs, TheDelegate rhs)
    {
        if(rhs != null)
        {
            lhs.TheEvent += rhs;
        }

        return lhs;
    }
    public static SmartEvent<T> operator -(SmartEvent<T> lhs, TheDelegate rhs)
    {
        if(rhs != null)
        {
            lhs.TheEvent += rhs;
        }

        return lhs;
    }

    public void Raise(T arg)
    {
        TheEvent?.Invoke(arg);
    }
}