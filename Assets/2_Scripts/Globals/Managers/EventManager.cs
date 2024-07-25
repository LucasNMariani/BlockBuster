using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeEvent
{
    GameOver,
    CompleteLevel,
    ResetLevel,
    Pause,
}

public static class EventManager
{
    public delegate void MethodEvent(params object[] parameters);
    static Dictionary<TypeEvent, MethodEvent> _events = new Dictionary<TypeEvent, MethodEvent>();

    public static void Subscribe(TypeEvent type, MethodEvent method)
    {
        if (_events.ContainsKey(type))
            _events[type] += method;
        else
            _events.Add(type, method);
    }

    public static void UnSubscribe(TypeEvent type, MethodEvent method)
    {
        if (_events.ContainsKey(type))
        {
            _events[type] -= method;

            if (_events[type] == null)
                _events.Remove(type);
        }
    }

    public static void Trigger(TypeEvent type, params object[] parameters)
    {
        if (_events.ContainsKey(type))
            _events[type](parameters);
    }
}