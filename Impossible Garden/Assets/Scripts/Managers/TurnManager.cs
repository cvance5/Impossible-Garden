using DG.Tweening;
using System.Collections;

public class TurnManager : Singleton<TurnManager>
{
    public static SmartEvent<int> BeginTurn = new SmartEvent<int>();
    public static SmartEvent<int> EndTurn = new SmartEvent<int>();

    public int TurnNumber;

    public override void Initialize()
    {
        TurnNumber = 0;
    }

    public void CompleteTurn()
    {
        StartCoroutine(AdvanceTurn());
    }

    public IEnumerator AdvanceTurn()
    {
        EndTurn.Raise(TurnNumber);
        yield return StartCoroutine(GardenManager.Instance.GrowAllPlants());

        TurnNumber++;

        BeginTurn.Raise(TurnNumber);
    }
}
