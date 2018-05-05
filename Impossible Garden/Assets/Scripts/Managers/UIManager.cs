using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public List<UIScreen> RegisteredScreens;
    public UIScreen ActiveScreen { get; private set; }

    public List<UIOverlay> RegisteredOverlays;
    public UIOverlay ActiveOverlay { get; private set; }

    public List<UIPopup> RegisteredPopups;
    public Stack<UIPopup> PopupStack { get; private set; }
    public UIPopup ActivePopup => PopupStack.Count > 0 ? PopupStack.Peek() : null;

    [Header("Layers")]
    public Transform OverlayLayer;
    public Transform ScreenLayer;
    public Transform PopupLayer;

    [Space]
    public GameObject ScrimLayer;

    public override void Initialize()
    {
        ActiveOverlay = null;
        ActiveScreen = null;
        PopupStack = new Stack<UIPopup>();
    }

    public T Get<T>() where T : UIObject
    {
        T uiObject;
        Type typeOfT = typeof(T);

        if (typeof(UIScreen).IsAssignableFrom(typeOfT))
            uiObject = GetScreen(typeOfT) as T;
        else if (typeof(UIOverlay).IsAssignableFrom(typeOfT))
            uiObject = GetOverlay(typeOfT) as T;
        else if (typeof(UIPopup).IsAssignableFrom(typeOfT))
            uiObject = GetPopup(typeOfT) as T;
        else if (typeof(UIActor).IsAssignableFrom(typeOfT))
            uiObject = CreateActor(typeOfT) as T;
        else
            throw new InvalidCastException("Type of " + typeOfT.ToString() + "  is not a UIObject!");

        return uiObject;
    }

    public void Show(UIObject objectToShow)
    {
        Type type = objectToShow.GetType();

        if (typeof(UIOverlay).IsAssignableFrom(type))
            ShowOverlay(objectToShow as UIOverlay);
        else if (typeof(UIPopup).IsAssignableFrom(type))
            UpdatePopupStack(objectToShow as UIPopup);
        else
            throw new InvalidCastException("Type of " + type.ToString() + "  is not a showable UI object!");
    }

    public T Create<T>() where T : UIObject
    {
        T uiObject;
        Type typeOfT = typeof(T);

        if (typeof(UIOverlay).IsAssignableFrom(typeOfT))
            uiObject = CreateOverlay(typeOfT) as T;
        else if (typeof(UIPopup).IsAssignableFrom(typeOfT))
            uiObject = CreatePopup(typeOfT) as T;
        else
            throw new InvalidCastException("Type of " + typeOfT.ToString() + "  is not a creatable UI object!");

        uiObject.SetVisible(false);

        return uiObject;
    }

    public void Clear(UIObject objectToClear)
    {
        Type type = objectToClear.GetType();

        if (typeof(UIPopup).IsAssignableFrom(type))
            ClearPopup(type);
        else
            throw new InvalidCastException("Type of " + type.ToString() + "  is not a removable UI object!");
    }

    public void SetVisibility<T>(bool isVisible)
    {
        Type typeOfT = typeof(T);

        if (typeof(UIOverlay).IsAssignableFrom(typeOfT))
            SetOverlayVisibility(typeOfT, isVisible);
        else
            throw new InvalidCastException("Type of " + typeOfT.ToString() + "  cannot have its visibility set by the manager!  Access the object directly instead.");
    }

    private void SetOverlayVisibility(Type type, bool isVisible)
    {
        if (ActiveOverlay.GetType() == type)
            SetActiveOverlay(ActiveOverlay);
        else
            Log.Warning("Overlay of type " + type.ToString() + " is not active!");
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

    private UIPopup GetPopup(Type type)
    {
        UIPopup selectedPopup = CreatePopup(type);
        UpdatePopupStack(selectedPopup);

        ScrimLayer.SetActive(selectedPopup.UseScrim);

        return selectedPopup;
    }

    private void ShowOverlay(UIOverlay overlay)
    {
        if (ActiveOverlay != overlay)
            SetActiveOverlay(overlay);
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

    private void SetActiveOverlay(UIOverlay overlay)
    {
        if (ActiveOverlay != null)
        {
            ActiveOverlay.SetVisible(false);
        }

        ActiveOverlay = overlay;
        ActiveOverlay.SetVisible(true);
    }

    private void UpdatePopupStack(UIPopup newPopup = null)
    {
        if (newPopup != null)
        {
            newPopup.transform.SetParent(PopupLayer, false);
            PopupStack.Push(newPopup);
        }
        else
        {
            UIPopup oldPopup = PopupStack.Pop();
            Destroy(oldPopup.gameObject);
        }

        foreach (UIPopup popup in PopupStack)
            popup.SetVisible(false);

        if (ActivePopup != null)
        {
            ActivePopup.SetVisible(true);
            ActivePopup.Activate();
            ScrimLayer.SetActive(ActivePopup.UseScrim);
        }
        else
        {
            ScrimLayer.SetActive(false);
        }
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
            throw new ArgumentOutOfRangeException("Could not find a screen of type " + type.ToString() + " in the screens listed!");

        return selectedScreen;
    }

    private UIOverlay CreateOverlay(Type type)
    {
        UIOverlay selectedOverlay = null;

        foreach (UIOverlay overlay in RegisteredOverlays)
            if (overlay.GetType() == type)
            {
                selectedOverlay = Instantiate(overlay.gameObject).GetComponent(type) as UIOverlay;
                selectedOverlay.transform.SetParent(OverlayLayer, false);
                break;
            }

        if (selectedOverlay == null)
            throw new ArgumentOutOfRangeException("Could not find an overlay of type " + type.ToString() + " in the overlays listed!");

        return selectedOverlay;
    }

    private UIPopup CreatePopup(Type type)
    {
        UIPopup selectedPopup = null;
        foreach (UIPopup popup in RegisteredPopups)
        {
            if (popup.GetType() == type)
            {
                selectedPopup = Instantiate(popup.gameObject).GetComponent(type) as UIPopup;
                break;
            }
        }

        if (selectedPopup == null)
            throw new ArgumentOutOfRangeException("Could not find a popup of type " + type.ToString() + " in the popups listed!");

        return selectedPopup;
    }

    private UIActor CreateActor(Type type)
    {
        GameObject newObject = new GameObject();
        newObject.transform.Reset();
        UIActor selectedActor = newObject.AddComponent(type) as UIActor;

        return selectedActor;
    }

    private void ClearPopup(Type type)
    {
        if (type == typeof(UIPopup) || type == ActivePopup.GetType())
            UpdatePopupStack();
        else
            throw new ArgumentException("Can't clear a popup of type " + type + " because it is not displayed.");
    }
}
