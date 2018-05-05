using System.Collections;

public class TurnManager : Singleton<TurnManager>
{
    public static SmartEvent<int> BeginTurn = new SmartEvent<int>();
    public static SmartEvent<int> CommitTurn = new SmartEvent<int>();
    public static SmartEvent<int> EndTurn = new SmartEvent<int>();

    public static SmartEvent<float> OnTick = new SmartEvent<float>();
    public static SmartEvent TurnTimeOut = new SmartEvent();

    public int TurnNumber { get; private set; }
    public float TurnTimer { get; private set; }
    private float _turnTimeLimit;

    public void Initialize(float turnTimeLimit)
    {
        TurnNumber = 0;
        _turnTimeLimit = turnTimeLimit;
        BeginTurn.Raise(TurnNumber);
        InitializeTimer();
    }

    public void CompleteTurn()
    {
        KillTimer();
        StartCoroutine(AdvanceTurn());
    }

    public IEnumerator AdvanceTurn()
    {
        CommitTurn.Raise(TurnNumber);
        yield return StartCoroutine(GardenManager.Instance.AdvanceTurn());

        EndTurn.Raise(TurnNumber);
        TurnNumber++;

        BeginTurn.Raise(TurnNumber);
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        TurnTimer = _turnTimeLimit;
        OnTick.Raise(TurnTimer);
        InvokeRepeating("InvokeTick", 1, 1);
    }

    private void InvokeTick()
    {
        TurnTimer--;
        OnTick.Raise(TurnTimer);

        if (TurnTimer <= 0)
        {
            TurnTimeOut.Raise();
            KillTimer();
        }
    }

    private void KillTimer()
    {
        CancelInvoke("InvokeTick");
        TurnTimer = 0;
    }
}
