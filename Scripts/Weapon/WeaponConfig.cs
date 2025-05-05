using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Resources/NewWeapon")]
public class WeaponConfig : Weapon, ISerializationCallbackReceiver
{
    public bool IsPlayerWeapon;
    public bool IsIgnoredAutoUpdate;
    [ShowIf("IsPlayerWeapon")] public int Cost;
    [ShowIf("IsPlayerWeapon")] public UIShopData.ShopWeapon.WeaponType WeaponType;
    [ShowIf("IsPlayerWeapon")] public UIShopData.ShopWeapon.WeaponState WeaponState;
    [ShowIf("IsPlayerWeapon")] public string Description;
    [ShowIf("IsPlayerWeapon")] public Sprite Icon;
    [HideInInspector] public string KEY_ID;
    public Mesh WeaponMesh;
    public AnimatorOverrideController AnimatorOverrideController;
    [HideInInspector] public AbilityBase SecondaryAbility;
    public List<AbilityBase> Abilities;
    [ShowIf("IsPlayerWeapon")] public List<AbilityBase> PossibleSecondaryAbilities;
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(KEY_ID)) KEY_ID = name;
        Abilities.ForEach(x => x.WEAPON_KEY_ID = KEY_ID);
        PossibleSecondaryAbilities.ForEach(x => x.WEAPON_KEY_ID = KEY_ID);
        Abilities.ForEach(x => x.WeaponType = WeaponType);
        PossibleSecondaryAbilities.ForEach(x => x.WeaponType = WeaponType);
        if (SecondaryAbility != null) SecondaryAbility.WEAPON_KEY_ID = KEY_ID;
        SecondaryAbility = Abilities.FirstOrDefault(x => x.WeaponAbilityType == UIShopData.ShopAbility.WeaponAbilityType.special);
    }

    private void OnValidate()
    {
        if (KEY_ID == string.Empty)
        {
            KEY_ID = this.name;
        }
    }
    //public WeaponConfig GetWeaponByMeta(int Meta)
    //{
    //    WeaponConfig weaponConfig = Instantiate(this);
    //    weaponConfig.SecondaryAbility = CloneAbilityWithBoostMeta(SecondaryAbility,Meta);
    //    weaponConfig.MainAbility = CloneAbilityWithBoostMeta(MainAbility,Meta);
    //    weaponConfig.Abilities = new();
    //    foreach(var ability in Abilities)
    //    {
    //        var cloneAbility = CloneAbilityWithBoostMeta(ability, Meta);
    //        weaponConfig.Abilities.Add(cloneAbility);
    //    }
    //    return weaponConfig;
    //}
    //public AbilityBase CloneAbilityWithBoostMeta(AbilityBase AbilityBase ,int Meta)
    //{
    //    var abilityClone = Instantiate(AbilityBase);
    //    abilityClone.AbilityBoostByMeta(Meta);
    //    return abilityClone;
    //}
}

public enum WeaponAttackType
{
    Melee,
    Ranged,
    AOE
}
public enum DirectionTypeF
{
    RightToLeft,
    LeftToRight,
    BackToForward,
    ForwardToBack
}
