using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScreen : UIObject
{
    public override void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public abstract void ActivateScreen();
}
