﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant
{
    public SmartEvent OnPlantDeath = new SmartEvent();

    public delegate bool PropogationCondition(Plot target);
    public PropogationCondition ShouldPropogate;

    public int GrowthStage;
    public int GrowthTimer;
    public Dictionary<int, int> StageDuration;

    public PlantManager Manager;

    public void Initialize()
    {
        GrowthStage = 0;
        GrowthTimer = 0;

        InitializeStageDuration();
        InitializePropogationCondition();
    }

    protected void InitializePropogationCondition()
    {
        ShouldPropogate = CheckPropogation;
    }
    protected abstract void InitializeStageDuration();
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