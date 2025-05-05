using System;
using System.Collections.Generic;

using Client;

using Statement;

using UniRx;

using Unity.Loading;

using UnityEngine;

/// <summary>
/// Entry point for player progress
/// </summary>
public class PlayerEntity : SourceEntity
{
    private static PlayerEntity _instance;
    public static PlayerEntity Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerEntity();
            return _instance;
        }
    }

    public PlayerWeapons Weapons;
    public PlayerAbilities Abilities;
    public RelicCollectionData RelicCollectionData;
    public PlayerCurrency Currency;
    public PerkCollectionData PerkCollectionData;
    public AbilityCollectionData AbilityCollectionData;
    public PlayerStatusData PlayerData;
    public GlobalMap Map;
    public PlayerEntity()
    {
        _instance = this;
        Currency = new PlayerCurrency();
        Weapons = new PlayerWeapons();
        Abilities = new PlayerAbilities();
        RelicCollectionData = new RelicCollectionData();
        PerkCollectionData = new PerkCollectionData();
        AbilityCollectionData = new AbilityCollectionData();
        PlayerData = new PlayerStatusData();
        Map = new GlobalMap();
    }
    public void Reset()
    {
        PerkCollectionData.Reset();
        AbilityCollectionData.Reset();
        RelicCollectionData.Reset();
    }

    public override SourceEntity Init()
    {

        //load selected slot data

        var weapons = ConfigModule.GetConfig<AllWeaponConfig>().GetPlayerWeapons();

        foreach (var weapon in weapons)
        {
            var addedWeapon = new WeaponPlayerData(weapon);

            if (addedWeapon.Status == UIShopData.ShopWeapon.WeaponState.Active || addedWeapon.Status == UIShopData.ShopWeapon.WeaponState.Default) Weapons.CurrentWeaponData = addedWeapon;

            if (Weapons.Weapons.Exists(x => x.KEY_ID == addedWeapon.KEY_ID)) continue;

            Weapons.Weapons.Add(addedWeapon);
        }


        var abilities = ConfigModule.GetConfig<AllAbilityConfig>().GetPlayerAbilities();

        foreach (var ability in abilities)
        {
            if (Abilities.Abilities.Exists(x => x.KEY_ID == ability.KEY_ID)) continue;

            Abilities.Abilities.Add(new AbilityPlayerData(ability));
        }

        var relics = ConfigModule.GetConfig<RelicConfig>().GetRelics();

        foreach (var relic in relics)
        {
            if (RelicCollectionData.Relics.Exists(x => x.KEY_ID == relic.KEY_ID)) continue;

            RelicCollectionData.Relics.Add(new RelicData(relic));
        }

        var playerData = SaveModule.GetData<SlotDataContainer>().GetCurrentData();

        Currency = playerData.Currency;
        Currency.Favour = 0;
        Weapons.LoadData(playerData.Weapons);
        Abilities.LoadData(playerData.Abilities);

        if (playerData.Map != null) Map = playerData.Map;
        else Map = new GlobalMap();

        ObserverEntity.Instance.Subscribe(ObserverEntity.Instance.OnPlayerChange, onPlayerInstantiate);
        return this;
    }

    void onPlayerInstantiate(ObserverPlayerWrapper observer)
    {
        observer.Subscribe(observer.OnPlayerChange, value =>
        {
            if (value != null)
            {
                value.Subscribe(value.OnHealthValueChange, healthValue =>
                {
                    PlayerData.Health = healthValue.CurrentValue;
                });
            }
        });
    }

    public void RequestBuyWeapon(string key)
    {
        var weaponData = Weapons.Weapons.Find(x => x.KEY_ID == key);

        if (weaponData == null) return;

        switch (weaponData.OnStatusChange.Value)
        {
            case UIShopData.ShopWeapon.WeaponState.BuyForCurrency:
                if (Currency.TryToSpend(PlayerCurrency.CurrencyType.Effigies, weaponData.Cost))
                {
                    weaponData.UpdateChangeStatus(UIShopData.ShopWeapon.WeaponState.Default);
                }
                break;
        }

        SaveModule.SaveSingleData<PlayerData>();
    }

    public void RequestSelectWeapon(string key)
    {
        var weaponData = Weapons.Weapons.Find(x => x.KEY_ID == key);

        if (weaponData == null) return;

        if (weaponData.OnStatusChange.Value == UIShopData.ShopWeapon.WeaponState.Default)
        {
            foreach (var weapon in Weapons.Weapons)
            {
                if(weapon.OnStatusChange.Value == UIShopData.ShopWeapon.WeaponState.Active) weapon.UpdateChangeStatus(UIShopData.ShopWeapon.WeaponState.Default);
            }

            Weapons.CurrentWeaponData = weaponData;

            Weapons.CurrentWeaponData.UpdateChangeStatus(UIShopData.ShopWeapon.WeaponState.Active);
        }

        var state = State.Instance;

        state.EcsRunHandler.World.GetPool<ChangeWeaponEvent>().Add(state.GetEntity("PlayerEntity")).weapon_ID = weaponData.KEY_ID;

        SaveModule.SaveSingleData<PlayerData>();
    }
    public void RequestBuyAbility(string key)
    {
        var ability = ConfigModule.GetConfig<AllAbilityConfig>().GetAbilityByID(key);

        if (ability == null) return;

        var buyedAbiltiy = Abilities.Abilities.Find(x => x.KEY_ID == ability.KEY_ID);

        switch (buyedAbiltiy.OnStatusChange.Value)
        {
            case UIShopData.ShopAbility.AbilityState.BuyForCurrency:
                if (Currency.TryToSpend(PlayerCurrency.CurrencyType.Effigies, ability.Cost))
                {
                    buyedAbiltiy.UpdateChangeStatus(UIShopData.ShopAbility.AbilityState.Default);
                }
                break;
        }

        SaveModule.SaveSingleData<PlayerData>();
    }
    public void RequestSelectAbility(string key)
    {
        var abilitySet = Abilities.Abilities.Find(x => x.KEY_ID == key);

        if (abilitySet == null) return;

        foreach (var ability in Abilities.Abilities)
        {
            if (ability.WEAPON_KEY_ID == abilitySet.WEAPON_KEY_ID && ability.AbilityType == UIShopData.ShopAbility.WeaponAbilityType.special)
            {
                if (ability.OnStatusChange.Value == UIShopData.ShopAbility.AbilityState.Active) ability.UpdateChangeStatus(UIShopData.ShopAbility.AbilityState.Default);
            }
        }

        var weapon = Weapons.Weapons.Find(x => x.KEY_ID == abilitySet.WEAPON_KEY_ID);

        if (weapon == null) return;

        weapon.CurrentSecondaryAbilityID = key;

        abilitySet.UpdateChangeStatus(UIShopData.ShopAbility.AbilityState.Active);

        SaveModule.SaveSingleData<PlayerData>();
    }

}

[System.Serializable]
public class PlayerCurrency
{
    public int Favour;
    public int Effigies;
    public int SkillShard;

    public PlayerCurrency()
    {
        Favour = 0;
        Effigies = 0;
        SkillShard = 0;
    }
    public void FavourChange(int value)
    {
        Favour = Mathf.Clamp(0, Favour += value, int.MaxValue);

        ObserverEntity.Instance.ChangeCurrencyValue(new CurrencyData(Favour, Effigies, SkillShard));
    }
    public void SkillShardChange(int value)
    {
        SkillShard = Mathf.Clamp(0, SkillShard += value, int.MaxValue);

        ObserverEntity.Instance.ChangeCurrencyValue(new CurrencyData(Favour, Effigies, SkillShard));
    }
    public void EffigiesChange(int value)
    {
        Effigies = Mathf.Clamp(0, Effigies += value, int.MaxValue);

        ObserverEntity.Instance.ChangeCurrencyValue(new CurrencyData(Favour, Effigies, SkillShard));
    }
    public bool TryToSpend(CurrencyType currencyType, int amount)
    {
        /*if (amount <= 0)
        {
            Debug.LogWarning("Сумма для списания должна быть положительной.");
            return false;
        }*/

        switch (currencyType)
        {
            case CurrencyType.Favour:
                if (Favour >= amount)
                {
                    FavourChange(-amount);
                    return true;
                }
                break;

            case CurrencyType.Effigies:
                if (Effigies >= amount)
                {
                    EffigiesChange(-amount);
                    return true;
                }
                break;

            case CurrencyType.SkillShard:
                if (SkillShard >= amount)
                {
                    SkillShardChange(-amount);
                    return true;
                }
                break;

            default:
                Debug.LogError($"Неизвестный тип валюты: {currencyType}");
                return false;
        }

        Debug.Log($"Недостаточно {currencyType} (нужно: {amount}, есть: {GetCurrencyValue(currencyType)})");
        return false;
    }

    public int GetCurrencyValue(CurrencyType currencyType)
    {
        return currencyType switch
        {
            CurrencyType.Favour => Favour,
            CurrencyType.Effigies => Effigies,
            CurrencyType.SkillShard => SkillShard,
            _ => throw new ArgumentOutOfRangeException(nameof(currencyType))
        };
    }

    public enum CurrencyType
    {
        Favour,
        Effigies,
        SkillShard
    }
}
[System.Serializable]
public class PlayerWeapons
{
    public WeaponPlayerData CurrentWeaponData;

    public List<WeaponPlayerData> Weapons;

    public PlayerWeapons()
    {
        Weapons = new List<WeaponPlayerData>();
    }

    public void LoadData(PlayerWeapons loadedData)
    {
        foreach (var weapon in loadedData.Weapons)
        {
            var existWeapon = Weapons.Find(x => x.KEY_ID == weapon.KEY_ID);

            if (existWeapon != null)
            {
                // �������� ������, � �� ������ ������ ������
                existWeapon.CopyFrom(weapon);
            }
            else
            {
                Weapons.Add(weapon);
            }
        }

        if(loadedData.CurrentWeaponData != null) CurrentWeaponData = loadedData.CurrentWeaponData;
    }
}
[System.Serializable]
public class PlayerAbilities
{
    public List<AbilityPlayerData> Abilities;
     
    public PlayerAbilities()
    {
        Abilities = new List<AbilityPlayerData>();
    }

    public void LoadData(PlayerAbilities loadedData)
    {
        foreach (var ability in loadedData.Abilities)
        {
            var existAbility = Abilities.Find(x => x.KEY_ID == ability.KEY_ID);

            if (existAbility != null)
            {
                // �������� ������, � �� ������ ������ ������
                existAbility.CopyFrom(ability);
            }
            else
            {
                Abilities.Add(ability);
            }
        }
    }
}
[System.Serializable]
public class WeaponPlayerData
{
    public int Cost;

    [NonSerialized] ReactiveProperty<UIShopData.ShopWeapon.WeaponState> _statusValue = new ReactiveProperty<UIShopData.ShopWeapon.WeaponState>();
    public IReactiveProperty<UIShopData.ShopWeapon.WeaponState> OnStatusChange  => _statusValue;

    public UIShopData.ShopWeapon.WeaponState Status;

    public string KEY_ID;

    public List<string> SecondaryAbilitiesID;
    public string CurrentSecondaryAbilityID;

    public WeaponPlayerData(WeaponConfig weaponConfig, UIShopData.ShopWeapon.WeaponState status = UIShopData.ShopWeapon.WeaponState.BuyForCurrency)
    {
        KEY_ID = weaponConfig.KEY_ID;

        SecondaryAbilitiesID = new List<string>();
        CurrentSecondaryAbilityID = weaponConfig.SecondaryAbility.KEY_ID;
        Status = weaponConfig.WeaponState;
        Cost = weaponConfig.Cost;
        _statusValue.Value = weaponConfig.WeaponState;

        foreach (var ability in weaponConfig.Abilities)
        {
            SecondaryAbilitiesID.Add(ability.KEY_ID);
        }
    }

    public void UpdateChangeStatus(UIShopData.ShopWeapon.WeaponState status)
    {
        _statusValue.Value = status;
        Status = status;
    }

    public void CopyFrom(WeaponPlayerData other)
    {
        this.KEY_ID = other.KEY_ID;
        this.Cost = other.Cost;
        this.SecondaryAbilitiesID = other.SecondaryAbilitiesID;
        this.CurrentSecondaryAbilityID = other.CurrentSecondaryAbilityID;
        this.Status = other.Status;
    }
}
[System.Serializable]
public class AbilityPlayerData
{
    [NonSerialized] ReactiveProperty<UIShopData.ShopAbility.AbilityState> _statusValue = new ReactiveProperty<UIShopData.ShopAbility.AbilityState>();
    public IReactiveProperty<UIShopData.ShopAbility.AbilityState> OnStatusChange => _statusValue;

    private UIShopData.ShopAbility.AbilityState _status;
    public UIShopData.ShopAbility.WeaponAbilityType AbilityType;

    public string KEY_ID;
    public string WEAPON_KEY_ID;
    public AbilityPlayerData(AbilityBase abilityBase, UIShopData.ShopAbility.AbilityState status = UIShopData.ShopAbility.AbilityState.BuyForCurrency)
    {
        KEY_ID = abilityBase.KEY_ID;
        WEAPON_KEY_ID = abilityBase.WEAPON_KEY_ID;
        _status = status;
        AbilityType = abilityBase.WeaponAbilityType;
        _statusValue.Value = status;
    }
    public void UpdateChangeStatus(UIShopData.ShopAbility.AbilityState status)
    {
        _statusValue.Value = status;
        _status = status;
    }
    public void CopyFrom(AbilityPlayerData other)
    {
        _status = other._status;
    }
}
[System.Serializable]
public class RelicCollectionData
{
    public List<RelicData> Relics;

    public List<CurrentRelicData> CurrentRelicData;

    public RelicCollectionData()
    {
        Relics = new List<RelicData>(); CurrentRelicData = new List<CurrentRelicData>();
    }
    public void Reset()
    {
        CurrentRelicData.Clear();
    }
    public void LoadData(RelicCollectionData loadedData)
    {
        foreach (var relic in loadedData.Relics)
        {
            var existRelic = Relics.Find(x => x.KEY_ID == relic.KEY_ID);

            if (existRelic != null)
            {
                // �������� ������, � �� ������ ������ ������
                existRelic.CopyFrom(relic);
            }
            else
            {
                Relics.Add(relic);
            }
        }
    }

    public void AddTemporaryRelic(CurrentRelicData currentRelicData)
    {
        foreach (var relicData in CurrentRelicData)
        {
            if (relicData.KEY_ID == currentRelicData.KEY_ID)
            {
                relicData.Level++;
                break;
            }
        } 

        CurrentRelicData.Add(currentRelicData);
    }
}
[System.Serializable]
public class CurrentRelicData
{
    public string KEY_ID;
    public int Level;

    public CurrentRelicData(string keyID)
    {
        KEY_ID = keyID;
    }
}
[System.Serializable]
public class RelicData
{
    public string KEY_ID;   
    public bool IsLocked;

    public RelicData(RelicResource relicBase, bool isLocked = true)
    {
        KEY_ID = relicBase.KEY_ID;
        IsLocked = isLocked;
    }
    public void CopyFrom(RelicData other)
    {
        this.IsLocked = other.IsLocked;
    }
}
[System.Serializable]
public class PerkCollectionData
{
    public List<CurrentPerkData> CurrentPerkData;

    public PerkCollectionData()
    {
        CurrentPerkData = new List<CurrentPerkData>();
    }
    public void Reset()
    {
        CurrentPerkData.Clear();
    }
    public void AddTemporaryPerk(CurrentPerkData currentRelicData)
    {
        foreach (var perkData in CurrentPerkData)
        {
            if (perkData.KEY_ID == currentRelicData.KEY_ID)
            {
                perkData.Level++;
                break;
            }
        }

        CurrentPerkData.Add(currentRelicData);
    }
}
[System.Serializable]
public class CurrentPerkData
{
    public string KEY_ID;
    public int Level;

    public CurrentPerkData(string id, int level)
    {
        KEY_ID = id;
        Level = level;
    }
    public void CopyFrom(CurrentPerkData other)
    {
        this.Level = other.Level;
    }
}
[System.Serializable]
public class AbilityCollectionData
{
    public List<CurrentAbilitiesData> CurrentAbilitiesData;

    public AbilityCollectionData()
    {
        CurrentAbilitiesData = new List<CurrentAbilitiesData>();
    }
    public void Reset()
    {
        CurrentAbilitiesData.Clear();
    }
    public void AddTemporaryAbility(CurrentAbilitiesData currentAbilityData)
    {
        // Проверяем существует ли ability с таким же ReferenceInput
        var existingByInput = CurrentAbilitiesData.Find(x => x.ReferenceInput == currentAbilityData.ReferenceInput);

        // Если нашли - заменяем его
        if (existingByInput != null)
        {
            CurrentAbilitiesData[CurrentAbilitiesData.IndexOf(existingByInput)] = currentAbilityData;
            return;
        }

        // Иначе проверяем существует ли ability с таким же KEY_ID
        var existingById = CurrentAbilitiesData.Find(x => x.KEY_ID == currentAbilityData.KEY_ID);

        if (existingById != null)
        {
            // Если нашли - увеличиваем уровень
            existingById.Level++;
        }
        else
        {
            // Если не нашли - добавляем новый
            CurrentAbilitiesData.Add(currentAbilityData);
        }
    }
}
[System.Serializable]
public class CurrentAbilitiesData
{
    public string ReferenceInput;
    public string KEY_ID;
    public int Level;

    public CurrentAbilitiesData(string id, int level, string inputReference)
    {
        KEY_ID = id;
        Level = level;
        ReferenceInput = inputReference;
    }
    public void CopyFrom(CurrentAbilitiesData other)
    {
        this.Level = other.Level;
    }
}
[System.Serializable]
public class ModifierCollectionData
{
    public List<CurrentModifierData> CurrentPerkData;

    public ModifierCollectionData()
    {
        CurrentPerkData = new List<CurrentModifierData>();
    }
    public void AddTemporaryModifier(CurrentModifierData currentRelicData)
    {
        foreach (var perkData in CurrentPerkData)
        {
            if (perkData.KEY_ID == currentRelicData.KEY_ID)
            {
                perkData.Level++;
                break;
            }
        }

        CurrentPerkData.Add(currentRelicData);
    }
}
[System.Serializable]
public class CurrentModifierData
{
    public string KEY_ID;
    public int Level;

    public CurrentModifierData(string id, int level)
    {
        KEY_ID = id;
        Level = level;
    }
    public void CopyFrom(CurrentModifierData other)
    {
        this.Level = other.Level;
    }
}
[System.Serializable]
public class PlayerStatusData
{
    public float Health;

    public PlayerStatusData()
    {
        Health = 0;
    }
}

public enum AbilityStatus { open, select, locked }