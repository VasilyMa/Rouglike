using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TiersConfig", menuName = "Configs/TiersConfig")]
public class UnitConfig : Config 
{
    public TierConfig[] TierConfigs;
    public TierConfig GetTierConfigByTierLevel(int tier)
    {
        var value = Mathf.Clamp(tier, 0, TierConfigs.Length - 1);
        return TierConfigs[value];
    }

    public override IEnumerator Init()
    {
        yield return null;
    }
}

