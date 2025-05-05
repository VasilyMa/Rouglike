using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ConditionConfig", menuName = "Config/ConditionConfig")]
public class ConditionConfig : Config
{
    public List<ConditionSettings> ConditionData;

    public override IEnumerator Init()
    {
        yield return null;
    }
}

