using System.Collections.Generic;
using UnityEngine;

public class SeedOverlay : UIOverlay
{
    public SeedFeeder MyFeeder { get; private set; }

    public Transform SeedSpawn;
    public Transform SeedDestroy;
    public Transform SeedHolder;

    private List<SeedActor> _actors;
    private Vector3 _seedMovement;

    public void Initialize(SeedFeeder myFeeder)
    {
        MyFeeder = myFeeder;
        MyFeeder.OnFeed += UIUpdate;
        _actors = new List<SeedActor>(SeedFeeder.NUMBER_SEED_SLOTS);
        _seedMovement = GetSeedMovement(SeedFeeder.NUMBER_SEED_SLOTS);
    }

    private void UIUpdate()
    {
        List<Seed> currentSeeds = MyFeeder.CurrentSeeds;

        for (int index = 0; index < _actors.Count; index++)
        {
            if (index == SeedFeeder.NUMBER_SEED_SLOTS - 1)
            {
                _actors[index].MoveNext(_seedMovement, true);
                _actors.RemoveAt(index);
            }
            else
            {
                _actors[index].MoveNext(_seedMovement);
            }
        }

        AddNewActor();
    }

    private void AddNewActor()
    {
        SeedActor newActor = UIManager.Instance.Get<SeedActor>();
        newActor.SetData(MyFeeder.NewestSeed);
        newActor.gameObject.name = newActor.Seed.SeedType.ToString();
        newActor.transform.SetParent(SeedHolder);
        newActor.transform.position = SeedSpawn.position;
        _actors.Insert(0, newActor);
    }

    private Vector3 GetSeedMovement(int numSeeds)
    {
        if (SeedSpawn == null)
        {
            throw new MissingReferenceException("Seed Spawn has not been assigned to the Seed Overlay.");
        }
        else if (SeedDestroy == null)
        {
            throw new MissingReferenceException("Seed Destroy has not been assigned to the Seed Overlay.");
        }

        Vector3 movement;

        float delta = 1f / numSeeds;
        movement = (SeedDestroy.position - SeedSpawn.position) * delta;

        return movement;
    }

    private void OnDestroy()
    {
        MyFeeder.OnFeed -= UIUpdate;
    }
}
