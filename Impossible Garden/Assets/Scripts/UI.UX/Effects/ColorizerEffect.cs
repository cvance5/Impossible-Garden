using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorizerEffect : UIEffect
{
    public Color Color;
    public float Duration;

    [SerializeField]
    private Image _target;
    private Tween _tween;

    public override void ApplyEffect()
    {
        _tween = _target.DOColor(Color, Duration).OnComplete(CompleteEffect);
    }

    public void ColorizeTo(Color color, float duration)
    {
        CompleteEffect();
        Color = color;
        Duration = duration;
        ApplyEffect();
    }

    public override void CompleteEffect()
    {
        _target.color = Color;
        ClearTween();
        OnComplete.Raise();
    }

    private void ClearTween()
    {
        if (_tween != null) _tween.Kill();
    }
}
