using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InnerInteractiveZoneMB : MonoBehaviour
{
    private UnityEvent _enterEvent, _exitEvent;
    private void Awake()
    {
        var obj = transform.parent.parent.GetComponent<InteractiveObject>();
        if (obj)
        {
            if (_enterEvent == null) _enterEvent = new UnityEvent();
            if (_exitEvent == null) _exitEvent = new UnityEvent();
            //_enterEvent.AddListener(obj.BecomeInteractive);
            //_exitEvent.AddListener(obj.StopBeingInteractive);
        }
    }
   /* private void OnTriggerEnter(Collider other)
    {
        _enterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _exitEvent?.Invoke();
    }

    private void OnDisable()
    {
        _enterEvent.RemoveAllListeners();
        _exitEvent.RemoveAllListeners();
    }*/
}
