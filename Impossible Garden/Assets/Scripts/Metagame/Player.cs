public class Player
{
    public User UserData { get; private set; }
    public Objective Objective { get; private set; }
    public int? Number { get; private set; }

    public Player(User user)
    {
        UserData = user;
    }

    public void SetObjective(Objective objective)
    {
        if (Objective == null)
            Objective = objective;
        else Log.Error(UserData.Username + " has more than one objective.");
    }

    public void SetNumber(int number)
    {
        if (Number == null)
            Number = number;
        else Log.Error(UserData.Username + " has more than one player number.");
    }

    public virtual void PrepareForTurn(bool hasControl) { }
}
