using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObserverPlayerWrapper
{
    private ReactiveProperty<PlayerObserver> _playerData = new ReactiveProperty<PlayerObserver>();
    public IReadOnlyReactiveProperty<PlayerObserver> OnPlayerChange => _playerData;

    public void AddPlayer(PlayerObserver playerObserver)
    {
        _playerData.Value = playerObserver;
    }

    public void RemovePlayer() 
    {
        _playerData.Value = null;
    }

    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }
}

public class ObserverBossWrapper
{
    private ReactiveProperty<BossObserver> _bossData = new ReactiveProperty<BossObserver>();
    public IReadOnlyReactiveProperty<BossObserver> OnBossChange => _bossData;

    public void AddBoss(BossObserver bossObserver)
    {
        _bossData.Value = bossObserver;
    }

    public void RemoveBoss()
    {
        _bossData.Value = null;
    }

    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }
}