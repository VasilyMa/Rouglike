using System;
using System.Collections;
using System.Collections.Generic;

using UniRx;

using UnityEngine;

public class InteractiveObserver
{
    private readonly InteractiveObject _interactiveObject;
    public InteractiveObject InteractiveObject => _interactiveObject;
    public InteractiveObserver(InteractiveObject interactiveObject)
    {
        _interactiveObject = interactiveObject;
    }


}