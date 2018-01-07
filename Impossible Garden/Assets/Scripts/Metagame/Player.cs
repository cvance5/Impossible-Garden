using UnityEngine.Networking;

public class Player
{
    public User UserData { get; private set; }
    public Objective GameObjective { get; private set; }

    public Player(User user)
    {
        UserData = user;
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

    public virtual void PrepareForTurn(bool hasControl) { }
}
