using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeedActor : UIActor, IPointerClickHandler
{
    public static SmartEvent<SeedActor> OnActorClicked = new SmartEvent<SeedActor>();

    public Seed Seed { get; private set; }

    private Image _icon;
    private Tween _currentTween = null;

    public override void SetData(object sourceData)
    {
        if (sourceData is Seed)
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

    public void MoveTo(Vector3 position, bool destroyOnComplete = false)
    {
        ClearTween();
        _currentTween = transform.DOMove(position, 1f);
        if (destroyOnComplete)
        {
            _currentTween.OnComplete(() => Destroy(gameObject));
        }
    }

    private void ClearTween()
    {
        if (_currentTween != null)
        {
            _currentTween.Complete();
            _currentTween = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnActorClicked.Raise(this);
    }
}