using System.Collections;
using UnityEngine;

public class LocalPlayer : Player
{
    public PlayerController Controller { get; private set; }
    public SeedFeeder Feeder { get; private set; }
    public SeedOverlay Overlay { get; private set; }

    public LocalPlayer(User user, PlayerController controller)
        : base(user)
    {
        Controller = controller;
        Controller.AssignOwner(this);
        if (Feeder == null)
        {
            Feeder = new SeedFeeder();
            SeedManager.RegisterFeeder(this, Feeder);
        }

        Overlay = UIManager.Instance.Create<SeedOverlay>();
        Overlay.Initialize(Feeder);
    }

    public override void PrepareForTurn(bool hasControl)
    {
        if (hasControl)
        {
            Controller.GainControl();
            Feeder.Feed();
        }
    }

    public IEnumerator SetHotseat()
    {
        yield return new WaitForEndOfFrame();
        TakeCamera();
    }

    public void ReleaseHotseat()
    {
        Controller.CameraController = null;
    }

    private void TakeCamera()
    {
        Camera.main.transform.position = Controller.transform.position;
        Controller.CameraController = Camera.main.GetComponent<CameraController>();
        Controller.CameraController.OrientTowards(GardenManager.Instance.ActiveGarden.Centerpoint);
        UIManager.Instance.Show(Overlay);
    }
}
