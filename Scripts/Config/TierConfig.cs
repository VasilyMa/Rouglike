using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TierConfig", menuName = "Configs/TierConfig")]
public class TierConfig : ScriptableObject
{
    public UnitMeta[] UnitConfigMetas;
    public UnitMetaDetail GetRandomUnit()
    {
        var unitConfigMeta = UnitConfigMetas[UnityEngine.Random.Range(0, UnitConfigMetas.Length)];
        return unitConfigMeta.GetUnit();
    }
}
[Serializable]
public class UnitMeta
{
    public EnemyMetaDataConfig enemyMetaDataConfig;
    public int Meta;
    public UnitMetaDetail GetUnit()
    {
        return enemyMetaDataConfig.GetUnit(Meta);
    }
}

