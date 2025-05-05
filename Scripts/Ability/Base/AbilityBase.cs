using AbilitySystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.Image;
using Client;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability/New")]
public class AbilityBase : ScriptableObject, ISerializationCallbackReceiver
{
    public bool IsPlayerAbility;
    [ShowIf("IsPlayerAbility")] public int Cost;
    [ShowIf("IsPlayerAbility")] public UIShopData.ShopAbility.WeaponAbilityType WeaponAbilityType;
    [HideInInspector] public UIShopData.ShopWeapon.WeaponType WeaponType;
    [ShowIf("IsPlayerAbility")] public UIShopData.ShopAbility.AbilityType AbilityType;
    [ShowIf("IsPlayerAbility")] public UIShopData.ShopAbility.AbilityState AbilityStatus;
    [HideInInspector] public string WEAPON_KEY_ID;
    [HideInInspector] public string KEY_ID;
    public Ability SourceAbility;
    public LoadedAbilitiesExecutor DownloadedData;
    public ModifierTags ModifierTags;


    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        SourceAbility.Update(IsPlayerAbility);
        if (string.IsNullOrEmpty(name)) return;

        KEY_ID = name;
    }
}

