using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public User UserData { get; private set; }
    public Objective GameObjective { get; private set; }
    public void SetObjective(Objective objective)
    {
        if(GameObjective == null)
        {
            GameObjective = objective;
        }
        else
        {
            Log.Error(UserData.Username + " has more than one objective.");
        }
    }

    public Player(User user)
    {
        UserData = user;
    }

}
