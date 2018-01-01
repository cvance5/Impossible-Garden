using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIOverlay : UIObject
{
    public override void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }    
}
