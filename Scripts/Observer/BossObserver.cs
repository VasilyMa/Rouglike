using UnityEngine;
using UniRx;
using System.Collections.Generic;
using Statement;
using System;

public class BossObserver
{
    private ReactiveProperty<EnemyMetaDataConfig> BossData = new ReactiveProperty<EnemyMetaDataConfig>();
    public IReadOnlyReactiveProperty<EnemyMetaDataConfig> OnBossValueChange => BossData;
    private ReactiveProperty<HealthValue> BossHealthValue = new ReactiveProperty<HealthValue>();
    public IReadOnlyReactiveProperty<HealthValue> OnHealthValueChange => BossHealthValue;
    private ReactiveProperty<ToughnessValue> BossToughnessbarValue = new ReactiveProperty<ToughnessValue>();
    public IReadOnlyReactiveProperty<ToughnessValue> OnToughnessValueChange => BossToughnessbarValue;
    private ReactiveProperty<BossStageValue> BossStageValue = new ReactiveProperty<BossStageValue>();
    public IReadOnlyReactiveProperty<BossStageValue> OnStageValueChange => BossStageValue;
    private ReactiveProperty<Vector2> BossPositionBarValue = new ReactiveProperty<Vector2>();
    public IReadOnlyReactiveProperty<Vector2> OnPositionValueChange => BossPositionBarValue;

    public void SetHealthValue(ReactiveProperty<HealthValue> healthValue)
    {
        BossHealthValue = healthValue;
    }
    public void SetToughnessValue(ReactiveProperty<ToughnessValue> toughnessValue)
    {
        BossToughnessbarValue = toughnessValue;
    }
    public void SetStageValue(ReactiveProperty<BossStageValue> stageValue)
    {
        BossStageValue = stageValue;
    }
    public void SetPositionValue(ReactiveProperty<Vector2> positionValue)
    {
        BossPositionBarValue = positionValue;
    }
    
    public void SetBossValue(EnemyMetaDataConfig enemyValue)
    {
        BossData.Value = enemyValue;
    }

    public virtual void Subscribe<T>(IObservable<T> observable, Action<T> onNext)
    {
        observable.Subscribe(onNext).AddTo(ObserverEntity.Instance.MainDisposable);
    }
}
public struct BossStageValue
{
    public int CurrentStage;
    public int MaxStage;  //Stage Count
    public BossStageValue(int currentValue, int maxValue)
    {
        CurrentStage = currentValue;
        MaxStage = maxValue;
    }
}