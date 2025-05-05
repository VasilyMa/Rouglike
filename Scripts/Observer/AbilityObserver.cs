using UnityEngine;
using UniRx;
using Statement;
using System;

public class AbilityObserver
{
    public string KEY_ID;
    private int abilityIndex;
    private string iconName;
    private string abilityInputActionName;
    public (int index, string icon, string key_id, string action_name) AbilityInfo => (abilityIndex, iconName, KEY_ID, abilityInputActionName);
    private ReactiveProperty<string> IconAbilityValue = new ReactiveProperty<string>();
    public IReadOnlyReactiveProperty<string> OnIconValueUpdate => IconAbilityValue;
    private ReactiveProperty<ChargeValue> ChargeAbilityValue = new ReactiveProperty<ChargeValue>();
    public IReadOnlyReactiveProperty<ChargeValue> OnChargeValueUpdate => ChargeAbilityValue;
    private ReactiveProperty<CooldownValue> CooldownAbilityValue = new ReactiveProperty<CooldownValue>();
    public IReadOnlyReactiveProperty<CooldownValue> OnCooldownUpdate => CooldownAbilityValue;
    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }
    public AbilityObserver(ReactiveProperty<CooldownValue> coodownValue, ReactiveProperty<ChargeValue> chargeValue, int index, string icon, string abilityInputAction)
    {
        CooldownAbilityValue = coodownValue;
        ChargeAbilityValue = chargeValue;
        abilityIndex = index;
        iconName = icon;
        abilityInputActionName = abilityInputAction;
    }

    public void SetNewIcon(string icon)
    {
        iconName = icon;
        IconAbilityValue.Value = icon;
    }
    public void SetNewActionName(string newInputActionName)
    {
        abilityInputActionName = newInputActionName;
    }

}

public struct CooldownValue
{
    public float MaxValue;
    public float CurrentValue;

    public CooldownValue(float currentValue, float maxValue)
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}
public struct ChargeValue
{
    public int MaxValue;
    public int CurrentValue;

    public ChargeValue(int currentValue, int maxValue)
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}