using DG.Tweening;
using UnityEngine;

public class FaderEffect : UIEffect
{
    public float Alpha;
    public float Duration;

    [SerializeField]
    private CanvasGroup _target;
    private Tween _tween;

    public override void ApplyEffect()
    {
        _tween = _target.DOFade(Alpha, Duration).OnComplete(CompleteEffect);
    }

    public void FadeTo(float alpha, float duration)
    {
        if (_tween != null) CompleteEffect();
        Alpha = alpha;
        Duration = duration;
        ApplyEffect();
    }

    public override void CompleteEffect()
    {
        _target.alpha = Alpha;
        ClearTween();
        OnComplete.Raise();
    }

    private void ClearTween()
    {
        if (_tween != null) _tween.Kill();
    }
}
