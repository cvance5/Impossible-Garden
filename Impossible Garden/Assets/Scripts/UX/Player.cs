using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public User UserData { get; private set; }
    public PlayerController Controller { get; private set; }
    public Objective GameObjective { get; private set; }

    public Player(User user, PlayerController controller)
    {
        UserData = user;
        Controller = controller;
    }

    public void SetObjective(Objective objective)
    {
        if (GameObjective == null)
        {
            GameObjective = objective;
        }
        else
        {
            Log.Error(UserData.Username + " has more than one objective.");
        }
    }

    public void SetControl(bool hasControl)
    {
        Controller.HasControl = hasControl;
    }

}
