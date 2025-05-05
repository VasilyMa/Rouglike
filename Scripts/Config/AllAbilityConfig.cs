using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "AllAbility", menuName = "Configs/AllAbility")]
public class AllAbilityConfig : Config, ISerializationCallbackReceiver
{
    [SerializeField] bool IsAutoUpdate;
    [SerializeField] bool HideWarning;
    [SerializeField] List<AbilityBase> abilityData = new List<AbilityBase>();

    private Dictionary<string, AbilityBase> abilityCollection;

    public override IEnumerator Init()
    {
        yield return UpdateData();
    }

    public AbilityBase GetAbilityByID(string key)
    {
        if (abilityCollection == null) UpdateData();

        if (abilityCollection.TryGetValue(key, out AbilityBase value))
        {
            return value;
        }

        throw new System.Exception($"Weapon with id {key} doens't exist in dictionary");
    }
    
    public bool TryGetAbilityByID(string key, out AbilityBase ability)
    {
        if (abilityCollection == null) UpdateData();

        ability = null;

        if (abilityCollection.TryGetValue(key, out AbilityBase value))
        {
            ability = value;
            return true;
        }

        throw new System.Exception($"Weapon with id {key} doens't exist in dictionary");
    }

    public List<AbilityBase> GetPlayerAbilities()
    {
        var abilities = new List<AbilityBase>();

        if (abilityCollection == null) UpdateData();

        foreach (var ability in abilityCollection)
        {
            if (ability.Value.IsPlayerAbility) abilities.Add(ability.Value);
        }

        return abilities;
    }

    public IEnumerator UpdateData()
    {
        if (abilityCollection == null) abilityCollection = new Dictionary<string, AbilityBase>();

        if (abilityData == null) abilityData = new List<AbilityBase>();

        AbilityBase[] abilities = Resources.LoadAll<AbilityBase>("Configs");

        if (abilities.Length == abilityCollection.Count)
        {
            if (!IsAutoUpdate) 
            yield return null;
        }
        
        if (abilities.Length > 0)
        {
            foreach (var ability in abilities)
            {
                if (!abilityData.Contains(ability)) abilityData.Add(ability);

                if (string.IsNullOrEmpty(ability.KEY_ID))
                {
                    if (HideWarning) continue;
                    
                    continue;
                }

                if (abilityCollection.ContainsKey(ability.KEY_ID)) continue;

                abilityCollection.Add(ability.KEY_ID, ability);

                
            }
        }
        else
        {
            foreach (var ability in abilityData)
            {
                if (string.IsNullOrEmpty(ability.KEY_ID))
                {
                    if (HideWarning) continue;
                    
                    continue;
                }

                if (abilityCollection.ContainsKey(ability.KEY_ID)) continue;

                abilityCollection.Add(ability.KEY_ID, ability);

                
            }
        }

        if (abilities.Length == abilityCollection.Count)
        {
            
            yield return null;
        }

        int lostItems = abilities.Length - abilityCollection.Count;

        

        yield return null;
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
