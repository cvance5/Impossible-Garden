using UnityEngine;

public class User
{
    public string Username;
    public uint UserID;

    public Color Color => Random.ColorHSV();

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
