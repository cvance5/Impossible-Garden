using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string Username;
    public uint UserID;

    public bool IsLocalUser;

    public User(string name, uint id)
    {
        Username = name;
        UserID = id;
    }

    public void AssignLocal(bool isLocal)
    {
        IsLocalUser = isLocal;
    }
}
