using UniRx;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObserverEntity : SourceEntity
{
    public CompositeDisposable MainDisposable = new CompositeDisposable();

    private static ObserverEntity _instance;
    public static ObserverEntity Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ObserverEntity();
            return _instance;
        }
    }

    //private ReactiveProperty<BossObserver> _bossData = new ReactiveProperty<BossObserver>();
    private ReactiveCollection<AbilityObserver> _abilitiesData = new ReactiveCollection<AbilityObserver>();
    private ReactiveCollection<EnemyObserver> _enemiesData = new ReactiveCollection<EnemyObserver>();
    private ReactiveCollection<InteractiveObserver> _interactiveData = new ReactiveCollection<InteractiveObserver>();

    private ReactiveProperty<GameResult> _gameResultData = new ReactiveProperty<GameResult>(new GameResult(0f,0,0f));
    private ReactiveProperty<GameStatus> _statusData = new ReactiveProperty<GameStatus>(new GameStatus());
    private ReactiveProperty<CurrencyData> _currencyData = new ReactiveProperty<CurrencyData>();
    private ReactiveProperty<ObserverPlayerWrapper> _observerPlayerData = new ReactiveProperty<ObserverPlayerWrapper>(new ObserverPlayerWrapper());
    private ReactiveProperty<ObserverBossWrapper> _observerBossData = new ReactiveProperty<ObserverBossWrapper>(new ObserverBossWrapper());
    public IReadOnlyReactiveCollection<InteractiveObserver> OnInteractiveChange => _interactiveData;
    public IReadOnlyReactiveProperty<CurrencyData> OnCurrencyData => _currencyData;
    public IReadOnlyReactiveProperty<GameStatus> OnStatusChange => _statusData;
    public IReadOnlyReactiveCollection<AbilityObserver> OnAbilitiesChange => _abilitiesData;
    public IReadOnlyReactiveCollection<EnemyObserver> OnEnemiesChange => _enemiesData;
    public IReadOnlyReactiveProperty<ObserverPlayerWrapper> OnPlayerChange => _observerPlayerData;
    public IReadOnlyReactiveProperty<ObserverBossWrapper> OnBossChange => _observerBossData;
    public IReadOnlyReactiveProperty<GameResult> OnGameResultChange => _gameResultData;

    public ObserverEntity()
    {
        if (_instance == null) _instance = this;
    }

    public override SourceEntity Init()
    {

        return this;
    }
    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(MainDisposable);
    }
    public virtual void Subscribe<T>(IObservable<T> observable, Action<T> onNext)
    {
        observable.Subscribe(onNext).AddTo(MainDisposable);
    }
    /// <summary>
    /// Subscribe for any list its be unit or abilities and etc. if true - for add, falise for remove element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reactiveCollection"></param>
    /// <param name="onChanged">If true react for add element - false for remoev element</param>
    public void SubscribeToList<T>(IReadOnlyReactiveCollection<T> reactiveCollection, Action<T, bool> onChanged)
    {
        reactiveCollection.ObserveAdd().Subscribe(addedItem => onChanged(addedItem.Value, true));
        reactiveCollection.ObserveRemove().Subscribe(removedItem => onChanged(removedItem.Value, false));
    }
    public void Dispose() => MainDisposable.Dispose();
    public void AddBoss(BossObserver bossObserver)
    {
        _observerBossData.Value.AddBoss(bossObserver);
    }
    public void RemoveBoss()
    {
        _observerBossData.Value.RemoveBoss();
    }
    public void AddPlayer(PlayerObserver playerObserver)
    {
        _observerPlayerData.Value.AddPlayer(playerObserver);
    }
    public void RemovePlayer()
    {
        _observerPlayerData.Value.RemovePlayer();
    }
    public void ClearTemporary()
    {
        _interactiveData.Clear();
        _abilitiesData.Clear();
        _enemiesData.Clear();
        RemoveBoss();
    }
    public void AddInteractive(InteractiveObserver interactiveObserver)
    {
        
        _interactiveData.Add(interactiveObserver);
    }
    public void RemoveInteractive(InteractiveObserver interactiveObserver) => _interactiveData.Remove(interactiveObserver);
    public void AddUnit(EnemyObserver enemyObserver) => _enemiesData.Add(enemyObserver);
    public void RemoveUnit(EnemyObserver enemyObserver) => _enemiesData.Remove(enemyObserver);
    public void AddAbility(AbilityObserver ability) => _abilitiesData.Add(ability);
    public void RemoveAbility(AbilityObserver ability) => _abilitiesData.Remove(ability);
    public void ChangeCurrencyValue(CurrencyData newCurrency) => _currencyData.Value = newCurrency;
    public void ChangeGameResultValue(GameResult result) => _gameResultData.Value = result; 
    public void UpdateEveryInteractive()
    {
        InteractiveObject nearestInteractive = null;
        float minDistance = float.MaxValue;

        // �������� �� ���� ������������� �������� � ���� ���������
        foreach (var observer in _interactiveData)
        {
            float distance = observer.InteractiveObject.CheckDistance();
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestInteractive = observer.InteractiveObject;
            }
        }

        // ��������� ����������: ������ ���������� ������� ���� true, ���� ��������� false
        foreach (var observer in _interactiveData)
        {
            observer.InteractiveObject.SetPriority(observer.InteractiveObject == nearestInteractive);
        }
    }
    public void SetNextStatus()
    {
        _statusData.Value.SetNext();
    }
}
public class GameResult
{
    public float GlobalMatchDuration;
    public float DPM; //damage per minute
    public int Kills;

    public GameResult(float matchDuration, int kill, float globalDamage)
    {
        GlobalMatchDuration = matchDuration;
        Kills = kill;
        DPM = globalDamage / (matchDuration / 60f);
    }
}

public struct CurrencyData
{
    public int Favour;
    public int Effigies;
    public int SkillShard;

    public CurrencyData(int favour, int effigies, int skillShard)
    {
        Favour = favour;
        Effigies = effigies;
        SkillShard = skillShard;
    }
}