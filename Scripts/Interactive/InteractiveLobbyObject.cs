using Client;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Statement;
using System;

public class InteractiveLobbyObject : InteractiveObject
{
    private Animator _animator;

    protected override void Awake()
    {

    }

    protected override void Dispose()
    {
        StopMoving();
    }
    public override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);

        StopMoving();
    }

    protected override void Init()
    {
        StartMoving();
    }

    void StartMoving()
    {
        if (_animator == null)
        {
            if(TryGetComponent<Animator>(out _animator))
            {
                _animator.SetBool("PlayerIsNearby", true);
            }

        }
        else
        {
            _animator.SetBool("PlayerIsNearby", true);
        }
    }
    void StopMoving()
    {
        if (_animator == null)
        {
            if (TryGetComponent<Animator>(out _animator))
            {
                _animator.SetBool("PlayerIsNearby", false);
            }
        }
        else
        {
            _animator.SetBool("PlayerIsNearby", false);
        }
    }

}
