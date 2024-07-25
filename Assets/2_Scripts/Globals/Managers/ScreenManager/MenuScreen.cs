using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour, IScreen
{
    public MenuScreen screenNext;
    public GameObject[] goToSetActive;
    public GameObject[] goToSetDesactive;

    public void BTN_Close()
    {
        ScreenManager.instance.Pop();
    }

    public void BTN_Active()
    {
        if (screenNext != null)
            ScreenManager.instance.ActiveScreen(screenNext);
    }

    public void Activate()
    {
        foreach (var go in goToSetActive)
        {
            go.SetActive(true);
        }
        foreach (var go in goToSetDesactive)
        {
            go.SetActive(false);
        }
        //gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        foreach (var go in goToSetActive)
        {
            go.SetActive(false);
        }
        foreach (var go in goToSetDesactive)
        {
            go.SetActive(true);
        }
        //gameObject.SetActive(false);
    }
}
