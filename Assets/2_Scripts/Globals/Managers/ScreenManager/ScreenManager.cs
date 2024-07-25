using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    Stack<IScreen> _screens = new Stack<IScreen>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pop();
    }

    public void ActiveScreen(IScreen screen)
    {
        screen.Activate();
        Push(screen);
    }

    public void Push(IScreen screen)
    {
        //if (_screens.Count > 0)
        //    _screens.Peek().Desactive();

        _screens.Push(screen);
    }

    public void Pop()
    {
        if (_screens.Count > 0)
        {
            _screens.Pop().Deactivate();

            if (_screens.Count > 0)
                _screens.Peek().Activate();
        }
    }
}
