using AbilitySystem;
using Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
[Serializable]
public class LoadedAbilitiesExecutor : IExecutor
{
    private static AbilityBase[] allConfigs;
    private string NameAbility;

    private float? Damage;
    private float? CoolDown;
    private float? Sheild;
    public float? DamageUp;
    public float? CoolDownUp;
    public float? SheildUp;
    public bool CheckFullData()
    {
        return true;
    }
    private AbilityBase FindAbilityByName(string name)
    {
        foreach (var config in allConfigs)
        {
            if (config.name == name)
            {
                return config;
            }
        }
        return null;
    }
    public void AbilityBoostByMeta(Ability ability, int MetaId)
    {
        foreach (var baseBlock in ability.BasicBlocks)
        {
            for (int i = 0; i < baseBlock.BasicComponents.Count; i++)
            {
                if (baseBlock.BasicComponents[i] is CoolDownComponent coolDown)
                {
                    if (CoolDown.HasValue)
                        coolDown.CoolDownValue = CoolDown.Value;
                    if (CoolDownUp.HasValue)
                        coolDown.CoolDownValue += MetaId * CoolDownUp.Value;
                    baseBlock.BasicComponents[i] = coolDown;
                }
            }
        }
        foreach (var resolveBlock in ability.ResolveBlocks)
        {
            for (int i = 0; i < resolveBlock.Components.Count; i++)
            {
                if (resolveBlock.Components[i] is DamageEffect damageComp)
                {
                    if (Damage.HasValue)
                        damageComp.DamageValue = Damage.Value;
                    if (DamageUp.HasValue)
                        damageComp.DamageValue += MetaId * DamageUp.Value;
                    resolveBlock.Components[i] = damageComp;
                }
            }
        }
        foreach (var timeLineBlock in ability.TimeLineBlocks)
        {
            for (int i = 0; i < timeLineBlock.FXComponents.Count; i++)
            {
                if (timeLineBlock.FXComponents[i] is RequestShieldEvent shieldEvent)
                {
                    if (Sheild.HasValue)
                        shieldEvent.DamageProtection = Sheild.Value;
                    if (SheildUp.HasValue)
                        shieldEvent.DamageProtection += MetaId * CoolDownUp.Value;
                    timeLineBlock.FXComponents[i] = shieldEvent;
                }
            }
        }
    }
    public void Invoke()
    {
        if (!CheckFullData()) return;
        if(allConfigs is null) allConfigs = Resources.LoadAll<AbilityBase>("");
        var abilityBase = FindAbilityByName(NameAbility);
        abilityBase.DownloadedData = (LoadedAbilitiesExecutor)this.MemberwiseClone();
        var ability = abilityBase.SourceAbility;
        foreach (var baseBlock in ability.BasicBlocks)
        {
            for (int i = 0; i < baseBlock.BasicComponents.Count; i++)
            {
                if (baseBlock.BasicComponents[i] is CoolDownComponent coolDown)
                {
                    if (CoolDown.HasValue)
                        coolDown.CoolDownValue = CoolDown.Value;
                    baseBlock.BasicComponents[i] = coolDown;
                }
            }
        }
        foreach (var resolveBlock in ability.ResolveBlocks)
        {
            for (int i = 0; i < resolveBlock.Components.Count; i++)
            {
                if (resolveBlock.Components[i] is DamageEffect damageComp)
                {
                    if (Damage.HasValue)
                        damageComp.DamageValue = Damage.Value;
                    resolveBlock.Components[i] = damageComp;
                }
            }
        }
        foreach (var timeLineBlock in ability.TimeLineBlocks)
        {
            for (int i = 0; i < timeLineBlock.FXComponents.Count; i++)
            {
                if (timeLineBlock.FXComponents[i] is RequestShieldEvent shieldEvent)
                {
                    if (Sheild.HasValue)
                        shieldEvent.DamageProtection = Sheild.Value;
                    timeLineBlock.FXComponents[i] = shieldEvent;
                }
            }
        }
    }

    public void SetData(params string[] data)
    {
        NameAbility = data[0];
        Damage = TryParse(data[1]);
        CoolDown = TryParse(data[2]);
        Sheild = TryParse(data[3]);
        DamageUp = TryParse(data[4]);
        CoolDownUp = TryParse(data[5]);
        SheildUp = TryParse(data[6]);
    }
    private float? TryParse(string data)
    {
        if (float.TryParse(data, out float result))
            return result;
        return null;
    }
}
