public abstract class Trait
{
    public Plant Owner { get; private set; }

    public void Assign(Plant owner)
    {
        Owner = owner;
        Initialize();
    }

    public void Initialize() { }
}
