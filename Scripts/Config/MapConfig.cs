using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Config/Map")]
public class MapConfig : Config
{
    public LocalMapGenerationConfig LocalMapGenerationConfig;
    public GlobalMapGenerationConfig GlobalMapGenerationConfig;
    

    public override IEnumerator Init()
    {
        yield return null;
    }
}
