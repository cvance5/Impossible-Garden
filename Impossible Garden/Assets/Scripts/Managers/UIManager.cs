using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public List<UIOverlay> Overlays;
    public UIOverlay ActiveOverlay { get; private set; }

    public Transform OverlayLayer; 

    public override void Initialize()
    {
        ActiveOverlay = null;
    }

    public T Get<T>() where T : UIObject
    {
        T uiObject;

        if(typeof(UIOverlay).IsAssignableFrom(typeof(T)))
        {
            uiObject = ShowOverlay(typeof(T)) as T;
        }
        else if (typeof(UIActor).IsAssignableFrom(typeof(T)))
        {
            uiObject = LoadActor(typeof(T)) as T;
        }
        else
        {
            throw new InvalidCastException("Type of " + typeof(T).ToString() + "  is not a UIObject!");
        }

        return uiObject;
    }

    public void Show(UIObject objectToShow)
    {
        if(typeof(UIOverlay).IsAssignableFrom(objectToShow.GetType()))
        {
            ShowOverlay(objectToShow as UIOverlay);
        }
    }

    public T Create<T>() where T : UIObject
    {
        T uiObject;

        if (typeof(UIOverlay).IsAssignableFrom(typeof(T)))
        {
            uiObject = LoadOverlay(typeof(T)) as T;
        }
        else
        {
            throw new InvalidCastException("Type of " + typeof(T).ToString() + "  is not a UIObject!");
        }

        uiObject.SetVisible(false);

        return uiObject;
    }

    public void SetVisibility<T>(bool isVisible)
    {
        if(typeof(UIOverlay).IsAssignableFrom(typeof(T)))
        {
            SetOverlayVisibility(typeof(T), isVisible);
        }
        else if (typeof(UIActor).IsAssignableFrom(typeof(T)))
        {
            Log.Warning("Cannot toggle visibility on actors.  Access the actor directly.");
        }
    }

    private void SetOverlayVisibility(Type type, bool isVisible)
    {
        if (ActiveOverlay.GetType() == type)
        {
            SetActiveOverlay(ActiveOverlay);
        }
        else
        {
            Log.Warning("Overlay of type " + type.ToString() + " is not active!");
        }
    }

    private UIOverlay ShowOverlay(Type type)
    {
        UIOverlay selectedOverlay = null;

        if(ActiveOverlay == null)
        {
            selectedOverlay = LoadOverlay(type);
        }
        else if(ActiveOverlay.GetType() != type)
        {
            ActiveOverlay.SetVisible(false);
            selectedOverlay = LoadOverlay(type);
        }
        else
        {
            SetOverlayVisibility(type, true);            
        }

        if(selectedOverlay != null)
        {
            SetActiveOverlay(selectedOverlay);
        }

        return selectedOverlay;
    }

    private void ShowOverlay(UIOverlay overlay)
    {
        if(ActiveOverlay != overlay)
        {
            SetActiveOverlay(overlay);
        }
    }

    private void SetActiveOverlay(UIOverlay overlay)
    {
        if(ActiveOverlay != null)
        {
            ActiveOverlay.SetVisible(false);
            ActiveOverlay.transform.SetParent(null, false);
        }

        ActiveOverlay = overlay;        
        ActiveOverlay.SetVisible(true);
        ActiveOverlay.transform.SetParent(OverlayLayer, false);
    }

    private UIOverlay LoadOverlay(Type type)
    {
        UIOverlay selectedOverlay = null;

        foreach(UIOverlay overlay in Overlays)
        {
            if(overlay.GetType() == type)
            {
                selectedOverlay = Instantiate(overlay.gameObject).GetComponent(type) as UIOverlay;
            }
        }

        if(selectedOverlay == null)
        {
            throw new ArgumentOutOfRangeException("Could not find an overlay of type " + type.ToString() + " in the overlays listed!");
        }

        return selectedOverlay;
    }

    private UIActor LoadActor(Type type)
    {
        GameObject newObject = new GameObject();
        UIActor selectedActor = newObject.AddComponent(type) as UIActor;

        return selectedActor;
    }
}
