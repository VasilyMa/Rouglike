using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Config : ScriptableObject
{
    public string Name { get; }
    public abstract IEnumerator Init();
    protected bool isLoaded;
}
