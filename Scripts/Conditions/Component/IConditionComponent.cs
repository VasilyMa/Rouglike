using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditionComponent 
{
    public void Invoke(int entityCondition, EcsWorld world);
}
