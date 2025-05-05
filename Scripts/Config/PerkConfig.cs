using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkConfig", menuName = "Config/Perk")]
public class PerkConfig : Config
{
    public List<Perk> _perksDataBase;
    private Dictionary<string, Perk> _dictionary;

    public override IEnumerator Init()
    {
        _dictionary = new Dictionary<string, Perk>();

        foreach (var perk in _perksDataBase)
        {
            _dictionary.Add(perk.KEY_ID, perk);   
        }

        yield return null;
    }

    public Perk GetPerkByID(string key)
    {
        return _dictionary[key];
    }
}
