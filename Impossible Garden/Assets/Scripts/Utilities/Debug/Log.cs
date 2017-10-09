﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Log
{ 
    public static void Error(string message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif 
    }
    public static void Warning(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
    public static void Info(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
