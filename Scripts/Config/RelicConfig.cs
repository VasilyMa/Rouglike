using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicConfig", menuName = "Configs/RelicConfig")]
public class RelicConfig : Config
{
    [Range(0, 1f)] public float ChanceDrop;
    [Range(0, 1f)] public float RareDrop;
    [Range(0, 1f)] public float CommonDrop;

    [SerializeField] List<RelicResource> RelicResources;
    Dictionary<string, RelicResource> _dictionary;
    public override IEnumerator Init()
    {
        _dictionary = new Dictionary<string, RelicResource>();

        foreach (var relic in RelicResources)
        {
            _dictionary.Add(relic.KEY_ID, relic);
        }

        yield return null;
    }

    public List<RelicResource> GetRelics()
    {
        return RelicResources;
    }

    public bool TryGetRelic(string key, out RelicResource relicResource)
    {
        if (_dictionary.TryGetValue(key, out relicResource))
        {
            return true;
        }

        relicResource = null;

        return false;
    }
    public RelicResource GetRelicByID(string key)
    {
        return RelicResources.Find(x => x.KEY_ID == key);
    }
}
