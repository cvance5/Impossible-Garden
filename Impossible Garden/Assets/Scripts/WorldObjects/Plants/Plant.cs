using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant
{
    public SmartEvent OnPlantDeath = new SmartEvent();

    public delegate bool PropogationCondition(Plot target);
    public PropogationCondition ShouldPropogate;

    public Dictionary<string, GameObject> PartsMap;
    public Dictionary<Type, Trait> TraitsMap = new Dictionary<Type, Trait>();

    public int GrowthStage;
    public int GrowthTimer;
    public List<int> StageDuration;

    public PlantActor Actor;

    protected Player _sower;

    public void Initialize()
    {
        GrowthStage = 0;
        GrowthTimer = 0;

        InitializeTraitsMap();
        InitializePartsMap();
        InitializeStageDuration();
        InitializePropogationCondition();
        InitializeData();
    }

    protected abstract void InitializeTraitsMap();
    protected abstract void InitializePartsMap();

    protected void InitializePropogationCondition()
    {
        ShouldPropogate = CheckPropogation;
    }

    protected void InitializeStageDuration()
    {
        StageDuration = new List<int>(GameManager.Instance.Settings.DurationMap[GetType()]);
    }

    public virtual void PreparePlantAppearance() { }

    public virtual void InitializeData() { }

    protected abstract bool CheckPropogation(Plot plot);

    public void SetSower(Player sower)
    {
        _sower = sower;
    }

    protected void AddTrait<T>() where T : Trait => AddTrait(typeof(T));
    protected void AddTrait(Type type)
    {
        if (TraitsMap.ContainsKey(type))
        {
            throw new ArgumentOutOfRangeException($"Trait of {type} already exists on {this}.");
        }
        else
        {
            TraitsMap.Add(type, Activator.CreateInstance(type) as Trait);
        }
    }

    public bool HasTrait<T>() where T : Trait => HasTrait(typeof(T));
    public bool HasTrait(Type type) => TraitsMap.ContainsKey(type);

    public T GetTrait<T>() where T : Trait => GetTrait(typeof(T)) as T;
    public Trait GetTrait(Type type) => TraitsMap[type];

    public void Grow()
    {
        GrowthTimer++;

        if (GrowthTimer > StageDuration[GrowthStage])
        {
            GrowthStage++;
            GrowthTimer = 0;

            if (GrowthStage >= StageDuration.Count)
            {
                Die();
            }
            else
            {
                ChangeGrowthStage();
            }
        }
        else
        {
            ApplyGrowthEffects();
        }
    }

    public void Wilt()
    {
        if (GrowthStage < StageDuration.Count - 1)
        {
            GrowthStage = StageDuration.Count - 1;
            ChangeGrowthStage();
            ApplyGrowthEffects();
        }
    }

    protected void Die()
    {
        OnPlantDeath.Raise();
    }

    protected abstract void ApplyGrowthEffects();
    protected abstract void ChangeGrowthStage();
}