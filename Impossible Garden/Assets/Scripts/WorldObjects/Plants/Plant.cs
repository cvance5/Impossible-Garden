using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant
{
    public SmartEvent OnPlantDeath = new SmartEvent();

    public delegate bool PropogationCondition(Plot target);
    public PropogationCondition ShouldPropogate;

    public Dictionary<string, GameObject> PartsMap;

    public int GrowthStage;
    public int GrowthTimer;
    public List<int> StageDuration;

    public PlantManager Manager;

    public void Initialize()
    {
        GrowthStage = 0;
        GrowthTimer = 0;

        InitializePlantPartsMap();
        InitializeStageDuration();
        InitializePropogationCondition();
    }

    protected abstract void InitializePlantPartsMap();
    protected void InitializePropogationCondition()
    {
        ShouldPropogate = CheckPropogation;
    }
    protected void InitializeStageDuration()
    {
        StageDuration = GameManager.Instance.Settings.DurationMap[GetType()];
    }

    public virtual void PreparePlantAppearance() { }
    protected abstract bool CheckPropogation(Plot plot);

    public void Grow()
    {        
        GrowthTimer++;

        if(GrowthTimer > StageDuration[GrowthStage])
        {
            GrowthStage++;
            GrowthTimer = 0;

            if(GrowthStage >= StageDuration.Count)
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