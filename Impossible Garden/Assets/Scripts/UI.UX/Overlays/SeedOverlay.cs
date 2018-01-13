using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedOverlay : UIOverlay
{
    public SeedFeeder MyFeeder { get; private set; }

    public Transform SeedSpawn;
    public Transform SeedDestroy;
    public Transform SeedHolder;

    public Image SelectionHighlight;

    private List<SeedActor> _actors;
    private SeedActor _selectedActor;
    private Vector3 _seedMovement;

    public void Initialize(SeedFeeder myFeeder)
    {
        MyFeeder = myFeeder;
        MyFeeder.OnFeed += UIUpdate;
        _actors = new List<SeedActor>(SeedFeeder.NUMBER_SEED_SLOTS);
        _seedMovement = GenerateSeedMovement(SeedFeeder.NUMBER_SEED_SLOTS);

        SeedActor.OnActorClicked += OnActorClicked;
        MyFeeder.OnSelectionChanged += OnSelectionChanged;
        MyFeeder.OnSeedRemoved += OnSeedRemoved;
    }

    private void OnSelectionChanged(int selection)
    {
        _selectedActor = _actors[selection];
        
        if(_selectedActor != null)
        {
            SelectionHighlight.gameObject.SetActive(true);
            SelectionHighlight.transform.position = _selectedActor.transform.position;
            SelectionHighlight.color = PlantColors.ColorByType(_selectedActor.Seed.SeedType);
        }
        else
        {
            SelectionHighlight.gameObject.SetActive(false);
        }
    }

    private void OnSeedRemoved(int consumed)
    {
        Destroy(_actors[consumed].gameObject);
    }

    private void OnActorClicked(SeedActor clickedActor)
    {
        MyFeeder.SetSeedSelection(_actors.IndexOf(clickedActor));
    }

    private void UIUpdate()
    {
        bool isFull = _actors.Count == SeedFeeder.NUMBER_SEED_SLOTS;

        for (int index = 0; index < _actors.Count; index++)
        {
            if(_actors[index] != null)
            {
                if (index == 0 && isFull)
                {
                    _actors[index].MoveNext(_seedMovement, true);
                }
                else
                {
                    _actors[index].MoveNext(_seedMovement);
                }
            }
        }

        if(isFull)
        {
            _actors.RemoveAt(0);
        }
        
        AddNewActor();

        if(_selectedActor == null)
        {
            MyFeeder.SetSeedSelection(_actors.Count - 1);
        }
    }

    private void AddNewActor()
    {
        SeedActor newActor = UIManager.Instance.Get<SeedActor>();
        newActor.SetData(MyFeeder.NewestSeed);
        newActor.gameObject.name = newActor.Seed.SeedType.ToString();
        newActor.transform.SetParent(SeedHolder);
        newActor.transform.position = SeedSpawn.position;
        _actors.Add(newActor);
    }

    private Vector3 GenerateSeedMovement(int numSeeds)
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
        if(MyFeeder != null)
        {
            MyFeeder.OnFeed -= UIUpdate;
        }        
    }
}
