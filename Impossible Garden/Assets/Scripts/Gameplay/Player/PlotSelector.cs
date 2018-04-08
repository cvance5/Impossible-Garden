using DG.Tweening;
using UnityEngine;

public class PlotSelector : MonoBehaviour
{
    public float EndValue = 0.5f;
    public float Duration = 2.0f;

    private Material _material;
    private HSVColor _sourceColor;

    private Transform _lookAtTarget;

    private Tween _emissionTween;

    public void Initialize(Color rbgColor)
    {
        _material = GetComponent<MeshRenderer>()?.material;
        _material.color = rbgColor;
        _sourceColor = new HSVColor(rbgColor)
        {
            v = 0
        };
        _material.SetColor(EmissionColor, HSVColor.HSVToRBG(_sourceColor));
        Deselect();
    }

    public void Select(Plot selectPlot)
    {
        transform.SetParent(selectPlot.transform, false);
        gameObject.SetActive(true);
    }

    public void ToggleCommit(bool isCommitted)
    {
        if (isCommitted)
        {
            if (_emissionTween == null)
            {
                HSVColor tweenColor = new HSVColor(_sourceColor);
                _emissionTween = DOTween.To(() => tweenColor.v, newValue => tweenColor.v = newValue, EndValue, Duration).
                    SetLoops(-1, LoopType.Yoyo).OnUpdate(() => _material.SetColor(EmissionColor, HSVColor.HSVToRBG(tweenColor)));
            }
        }
        else
        {
            if (_emissionTween != null)
            {
                _emissionTween.Rewind();
                _emissionTween.Kill();
                _emissionTween = null;
                _material.SetColor(EmissionColor, Color.black);
            }
        }
    }

    public void Deselect()
    {
        gameObject.SetActive(false);
    }

    private const string EmissionColor = "_EmissionColor";
}
