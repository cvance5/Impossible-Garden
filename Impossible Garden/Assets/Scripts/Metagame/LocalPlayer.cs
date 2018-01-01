using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player {

    public PlayerController Controller { get; private set; }
    public SeedFeeder Feeder { get; private set; }
    public SeedOverlay Overlay { get; private set; }

    public LocalPlayer(User user, PlayerController controller)
        : base(user)
    {
        Controller = controller;
        if (Feeder == null)
        {
            Feeder = new SeedFeeder();
            SeedManager.RegisterFeeder(this, Feeder);
        }

        Overlay = UIManager.Instance.Create<SeedOverlay>();
        Overlay.Initialize(Feeder);
    }

    public override void SetControl(bool hasControl)
    {
        Controller.HasControl = hasControl;

        if(hasControl)
        {
            Camera.main.transform.position = Controller.transform.position;
            Camera.main.transform.LookAt(GardenManager.Instance.ActiveGarden.Centerpoint);
            UIManager.Instance.Show(Overlay);
            Feeder.Feed();
        }
    }
}
