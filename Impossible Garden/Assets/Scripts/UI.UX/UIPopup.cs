public class UIPopup : UIObject
{
    public bool UseScrim;

    public override void SetVisible(bool isVisible)
    {
        this.SetActive(isVisible);
    }
}
