using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "AllWeapon", menuName = "Configs/AllWeapon")]
public class AllWeaponConfig : Config, ISerializationCallbackReceiver
{
    [SerializeField] bool IsAutoUpdate;
    [SerializeField] bool HideWarning;

    [SerializeField] List<WeaponConfig> weaponsData = new List<WeaponConfig>();

    private Dictionary<string, WeaponConfig> weaponCollections;

    public override IEnumerator Init()
    {
        yield return UpdateData();
    }

    public WeaponConfig GetWeaponByID(string key)
    {
        if (weaponCollections == null) UpdateData();

        if (weaponCollections.TryGetValue(key, out WeaponConfig value))
        {
            return value;
        }

        throw new System.Exception($"Weapon with id {key} doens't exist in dictionary");
    }

    public List<WeaponConfig> GetPlayerWeapons()
    {
        var weapons = new List<WeaponConfig>();

        if (weaponCollections == null) UpdateData();

        foreach (var weapon in weaponCollections)
        {
            if (weapon.Value.IsPlayerWeapon) weapons.Add(weapon.Value);
        }

        return weapons;
    }

    public IEnumerator UpdateData()
    {
        if (weaponCollections == null) weaponCollections = new Dictionary<string, WeaponConfig>();

        if (weaponsData == null) weaponsData = new List<WeaponConfig>();

        WeaponConfig[] weapons = Resources.LoadAll<WeaponConfig>("Configs");

        if (weapons.Length == weaponCollections.Count)
        {
            if (!IsAutoUpdate) 

            yield return null;
        }

        if (weapons.Length > 0)
        {
            foreach (var weapon in weapons)
            {
                if (weapon.IsIgnoredAutoUpdate)
                    yield return null;

                if (!weaponsData.Contains(weapon)) weaponsData.Add(weapon);

                if (string.IsNullOrEmpty(weapon.KEY_ID))
                {
                    if (HideWarning) continue;
                    
                    continue;
                }

                if (weaponCollections.ContainsKey(weapon.KEY_ID)) continue;

                weaponCollections.Add(weapon.KEY_ID, weapon);

                
            }
        }
        else
        {
            foreach (var weapon in weaponsData)
            {
                if (string.IsNullOrEmpty(weapon.KEY_ID))
                {
                    if (HideWarning) continue;
                    
                    continue;
                }

                if (weaponCollections.ContainsKey(weapon.KEY_ID)) continue;

                weaponCollections.Add(weapon.KEY_ID, weapon);

                
            }
        }

        if (weapons.Length == weaponCollections.Count)
        { 
            
            yield return null; 
        }

        int lostItems = weapons.Length - weaponCollections.Count;

        

        yield return null;
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
