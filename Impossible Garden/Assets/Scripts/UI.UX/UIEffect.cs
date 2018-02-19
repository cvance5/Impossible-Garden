public abstract class UIEffect : UIObject
{
    public static SmartEvent OnComplete = new SmartEvent();

    public override void SetVisible(bool isVisible)
    {
        if (isVisible) ApplyEffect();
        else CompleteEffect();
    }

    public abstract void ApplyEffect();
    public abstract void CompleteEffect();
}
