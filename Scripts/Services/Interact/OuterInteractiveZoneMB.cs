using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OuterInteractiveZoneMB : MonoBehaviour
{
    private UnityEvent _enterEvent, _exitEvent;
    private void Awake()
    {
        var obj = transform.parent.GetComponent<InteractiveObject>();
        if (obj)
        {
            if (_enterEvent == null) _enterEvent = new UnityEvent();
            if (_exitEvent == null) _exitEvent = new UnityEvent();
            //_enterEvent.AddListener(obj.StartMoving);
            //_exitEvent.AddListener(obj.StopMoving);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_enterEvent != null) 
        _enterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _exitEvent?.Invoke();
    }

    private void OnDisable()
    {
        _enterEvent?.RemoveAllListeners();
        _exitEvent?.RemoveAllListeners();
    }
}
