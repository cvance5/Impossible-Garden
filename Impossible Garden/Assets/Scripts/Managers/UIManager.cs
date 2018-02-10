using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public List<UIScreen> RegisteredScreens;
    public UIScreen ActiveScreen { get; private set; }

    public List<UIOverlay> RegisteredOverlays;
    public UIOverlay ActiveOverlay { get; private set; }

    [Header("Layers")]
    public Transform OverlayLayer;
    public Transform ScreenLayer;

    public override void Initialize()
    {
        ActiveOverlay = null;
        ActiveScreen = null;
    }

    public T Get<T>() where T : UIObject
    {
        T uiObject;

        if (typeof(UIOverlay).IsAssignableFrom(typeof(T)))
        {
            uiObject = GetOverlay(typeof(T)) as T;
        }
        else if (typeof(UIActor).IsAssignableFrom(typeof(T)))
        {
            uiObject = CreateActor(typeof(T)) as T;
        }
        else if (typeof(UIScreen).IsAssignableFrom(typeof(T)))
        {
            uiObject = GetScreen(typeof(T)) as T;
        }
        else
        {
            throw new InvalidCastException("Type of " + typeof(T).ToString() + "  is not a UIObject!");
        }

        return uiObject;
    }

    public void Show(UIObject objectToShow)
    {
        var type = objectToShow.GetType();

        if (typeof(UIOverlay).IsAssignableFrom(type))
        {
            ShowOverlay(objectToShow as UIOverlay);
        }
    }

    public T Create<T>() where T : UIObject
    {
        T uiObject;

        if (typeof(UIOverlay).IsAssignableFrom(typeof(T)))
        {
            uiObject = CreateOverlay(typeof(T)) as T;
        }
        else
        {
            throw new InvalidCastException("Type of " + typeof(T).ToString() + "  is not a creatable UI object!");
        }

        uiObject.SetVisible(false);

        return uiObject;
    }

    public void SetVisibility<T>(bool isVisible)
    {
        if (typeof(UIOverlay).IsAssignableFrom(typeof(T)))
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

    private UIOverlay GetOverlay(Type type)
    {
        UIOverlay selectedOverlay = null;

        if (ActiveOverlay == null)
            selectedOverlay = CreateOverlay(type);
        else if (ActiveOverlay.GetType() != type)
        {
            ActiveOverlay.SetVisible(false);
            selectedOverlay = CreateOverlay(type);
        }
        else
            SetOverlayVisibility(type, true);

        if (selectedOverlay != null)
            SetActiveOverlay(selectedOverlay);

        return selectedOverlay;
    }

    private UIScreen GetScreen(Type type)
    {
        UIScreen selectedScreen = null;

        if (ActiveScreen == null)
            selectedScreen = CreateScreen(type);
        else if (ActiveScreen.GetType() != type)
        {
            ActiveScreen.SetVisible(false);
            selectedScreen = CreateScreen(type);
        }
        else
            ActiveScreen.SetVisible(true);

        if (selectedScreen != null)
            SetActiveScreen(selectedScreen);

        return selectedScreen;
    }

    private void ShowOverlay(UIOverlay overlay)
    {
        if (ActiveOverlay != overlay)
        {
            SetActiveOverlay(overlay);
        }
    }

    private void SetActiveOverlay(UIOverlay overlay)
    {
        if (ActiveOverlay != null)
        {
            ActiveOverlay.SetVisible(false);
            ActiveOverlay.transform.SetParent(null, false);
        }

        ActiveOverlay = overlay;
        ActiveOverlay.SetVisible(true);
        ActiveOverlay.transform.SetParent(OverlayLayer, false);
    }

    private void SetActiveScreen(UIScreen screen)
    {
        if (ActiveScreen != null)
        {
            ActiveScreen.SetVisible(false);
            ActiveScreen.transform.SetParent(null, false);
        }

        ActiveScreen = screen;
        ActiveScreen.SetVisible(true);
        ActiveScreen.transform.SetParent(ScreenLayer, false);
        ActiveScreen.ActivateScreen();
    }

    private UIOverlay CreateOverlay(Type type)
    {
        UIOverlay selectedOverlay = null;

        foreach (UIOverlay overlay in RegisteredOverlays)
            if (overlay.GetType() == type)
            {
                selectedOverlay = Instantiate(overlay.gameObject).GetComponent(type) as UIOverlay;
                break;
            }

        if (selectedOverlay == null)
            throw new ArgumentOutOfRangeException("Could not find an overlay of type " + type.ToString() + " in the overlays listed!");

        return selectedOverlay;
    }

    private UIScreen CreateScreen(Type type)
    {
        UIScreen selectedScreen = null;

        foreach (UIScreen screen in RegisteredScreens)
            if (screen.GetType() == type)
            {
                selectedScreen = Instantiate(screen.gameObject).GetComponent(type) as UIScreen;
                break;
            }

        if (selectedScreen == null)
            throw new ArgumentOutOfRangeException("Could not find an screen of type " + type.ToString() + " in the screens listed!");

        return selectedScreen;
    }

    private UIActor CreateActor(Type type)
    {
        GameObject newObject = new GameObject();
        newObject.transform.Reset();
        UIActor selectedActor = newObject.AddComponent(type) as UIActor;

        return selectedActor;
    }
}
