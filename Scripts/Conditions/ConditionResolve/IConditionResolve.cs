using JetBrains.Annotations;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditionResolve 
{
    public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world);
    public void Recalculate(float charge);
}
