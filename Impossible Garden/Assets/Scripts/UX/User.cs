using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string Username;
    public uint UserID;

    public User(string name, uint id)
    {
        Username = name;
        UserID = id;
    }
}
