using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedActor : UIActor
{    
    public Seed Seed { get; private set; }

    private Image _icon;
    private Tween _currentTween = null;

    public override void SetData(object sourceData)
    {
        if(sourceData is Seed)
        {
            Seed = sourceData as Seed;
            _icon = gameObject.AddComponent<Image>();
            _icon.sprite = Seed.Icon;
        }
        else
        {
            throw new ArgumentException("Source data must be a seed!");
        }
    }

    public void MoveNext(Vector3 movement, bool destroyOnComplete = false)
    {
        ClearTween();
        _currentTween = transform.DOMove(transform.position + movement, 1f);
        if(destroyOnComplete)
        {
            _currentTween.OnComplete(() => Destroy(gameObject));
        }
    }

    private void ClearTween()
    {
        if(_currentTween != null)
        {
            _currentTween.Complete();
            _currentTween = null;
        }
    }
}