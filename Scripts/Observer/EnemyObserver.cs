using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using System;
using Client;
using Statement;

public class EnemyObserver
{
    public bool IsValidate;
    private ReactiveProperty<HealthValue> EnemyHealthbarValue = new ReactiveProperty<HealthValue>();
    public IReadOnlyReactiveProperty<HealthValue> OnHealthChange => EnemyHealthbarValue;

    private ReactiveProperty<ToughnessValue> EnemyToughnessbarValue = new ReactiveProperty<ToughnessValue>();
    public IReadOnlyReactiveProperty<ToughnessValue> OnToughnessChange => EnemyToughnessbarValue;

    private ReactiveProperty<Vector2> EnemyPositionBarValue = new ReactiveProperty<Vector2>();
    public IReadOnlyReactiveProperty<Vector2> OnPositionUpdate => EnemyPositionBarValue;
    
    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }

    public EnemyObserver(ReactiveProperty<HealthValue> healthValue, ReactiveProperty<ToughnessValue> toughnessValue, ReactiveProperty<Vector2> position)
    {
        EnemyHealthbarValue = healthValue;
        EnemyToughnessbarValue = toughnessValue;
        EnemyPositionBarValue = position;
    }

    /*private void DeleteUnitBarOnDeath(HealthValue value)
    {
        if (!barExists) return;
        if (value.CurrentValue <= 0)
        {
            _uiInGameInterfaceAttributes.DeleteEnemyBar(_enemyName);
            barExists = false;
        }
    }
    private void UpdateHPBarValue(HealthValue value)
    {
        if (!barExists) return;
        _uiInGameInterfaceAttributes.SetValueEnemyBar(newEnemyBarItem, EnemyBarItem.ValueType.Health, value.CurrentValue, value.MaxValue);
    }
    private void UpdateToughnessBarValue(ToughnessValue value)
    {
        if (!barExists) return;
        if (value.MaxValue > 0)
        {
            _uiInGameInterfaceAttributes.SetValueEnemyBar(newEnemyBarItem, EnemyBarItem.ValueType.Toughness, value.CurrentValue, value.MaxValue);
        }
    }
    private void UpdateBarPosition(Vector2 value)
    {
        if (!barExists) return;
        _uiInGameInterfaceAttributes.SetPositionEnemyBar(newEnemyBarItem, value.x, value.y);

    }
    private void UpdateDamageEventPosition(Vector2 value)
    {
        if (newWorldspaceActivity.Element != null)
        {
            _uiInGameInterfaceAttributes.SetPositionWorldspaceObject(newWorldspaceActivity, value.x, value.y);
        }
    }
    private void VisualiseDamage(HealthValue value)
    {
        if (_previousHP == value.CurrentValue) return;
        newWorldspaceActivity = _uiInGameInterfaceAttributes.CreateDamageVisualisation(((int)(_previousHP - value.CurrentValue)).ToString(), WorldspaceActivity.Type.Damage);
        _previousHP = value.CurrentValue;
    }*/

    //public EnemyBarItem newEnemyBarItem;
    //public WorldspaceActivity newWorldspaceActivity;
    //private UIManagerRitualist.UIInGameInterface _uiInGameInterfaceAttributes;
    private static int enemyCount = 0;
    private string _enemyName;
    private float _previousHP;
    private bool barExists;

    /*public void Init(HealthValue initialHealthValue)
    {
        _state = BattleState.Instance;
        _uiInGameInterfaceAttributes = UIManagerRitualist.GetUIManager.UIInGameInterfaceComponents;
        _enemyName = "Enemy" + enemyCount++;
        newEnemyBarItem = _uiInGameInterfaceAttributes.AddEnemyBar(_enemyName);
        barExists = true;
        EnemyHealthbarValue.Value = initialHealthValue;
        _previousHP = initialHealthValue.CurrentValue;
        EnemyHealthbarValue.Subscribe(
            UpdateHPBarValue
            ).AddTo(_state.MainDisposable);
        EnemyHealthbarValue.Subscribe(
            VisualiseDamage
            ).AddTo(_state.MainDisposable);
        EnemyHealthbarValue.Subscribe(
            DeleteUnitBarOnDeath
            ).AddTo(_state.MainDisposable);

        EnemyToughnessbarValue.Subscribe(
            UpdateToughnessBarValue
            ).AddTo(_state.MainDisposable);

        EnemyPositionBarValue.Subscribe(
            UpdateBarPosition
            ).AddTo(_state.MainDisposable);
        EnemyPositionBarValue.Subscribe(
            UpdateDamageEventPosition
            ).AddTo(_state.MainDisposable);
    }*/
    
}


public struct ToughnessValue
{
    public float CurrentValue;
    public float MaxValue;

    public ToughnessValue(float currentValue, float maxValue)
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }
}