using UnityEngine;
using UniRx;
using Statement;
using System;

public class PlayerObserver
{
    private ReactiveProperty<HealthValue> HealthValue = new ReactiveProperty<HealthValue>();
    public IReadOnlyReactiveProperty<HealthValue> OnHealthValueChange => HealthValue;

    public void SetPlayerProperty(ReactiveProperty<HealthValue> healthValue)
    {
        HealthValue = healthValue;
    }

    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }
}

public struct HealthValue
{
    public float CurrentValue;
    public float MaxValue;

    public HealthValue(float currentValue, float maxValue)
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}
public struct StaminaValue
{
    public float CurrentValue;
    public float MaxValue;

    public StaminaValue(float currentValue, float maxValue)
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}
