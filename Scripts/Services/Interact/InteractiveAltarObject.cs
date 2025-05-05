using Client;
using Leopotam.EcsLite;
using Statement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using Sirenix.Utilities;

public class InteractiveAltarObject : InteractiveObject
{
    public DropConfig dropConfig;

    [Header("Dissolve the HotKey")] public KeyCode RerollKeyCode = KeyCode.R;
    private Subject<KeyCode> onInputButtonReroll = new Subject<KeyCode>();
    public IObservable<KeyCode> OnButtonRerollPressed => onInputButtonReroll;

    private ReactiveProperty<CallbackType> _callback = new ReactiveProperty<CallbackType>();
    public IReactiveProperty<CallbackType> OnCallback => _callback;
    private ReactiveCollection<Perk> _reactivePerks = new ReactiveCollection<Perk>();
    public IReadOnlyCollection<Perk> OnReactivePerks => _reactivePerks;

    public Action ActionReroll;
    public Action<string> ActionInvokeReward;

    protected override void OnEnable()
    {
        base.OnEnable();

        ActionReroll += TryToReroll;
        ActionInvokeReward += InvokeReward;

        Observable.EveryUpdate()
             .Where(_ => _priorityValue.Value)
             .Where(_ => _target != null)
             .Where(_ => Input.GetKeyDown(RerollKeyCode))
            .Subscribe(_ =>
            {
                TryToReroll();
            })
            .AddTo(disposables);

        InvokeReroll();
    }

    void TryToReroll()
    {
        if (PlayerEntity.Instance.Currency.TryToSpend(PlayerCurrency.CurrencyType.Effigies, 1))
        {
            InvokeReroll();
            _callback.Value = CallbackType.succsess;
            return;
        }
        _callback.Value = CallbackType.denied;
    }

    void InvokeReroll()
    {
        int Count = -1;

        List<Perk> dropLoot = new();
        var newLoot = dropConfig.GetDropLoot(Count);
        foreach (var dropPerk in newLoot)
        {
            if (dropPerk is not DropPerk) continue;
            var perk = (dropPerk as DropPerk).Perk;
            dropLoot.Add(perk);
        }
        _reactivePerks.Clear();
        _reactivePerks.AddRange(dropLoot);
    }

    void InvokeReward(string key)
    {
        var item = _reactivePerks.First(x => x.KEY_ID == key);

        if (item is not null)
        {
            int requestEntity = BattleState.Instance.EcsRunHandler.World.NewEntity();
            ref var requestComp = ref BattleState.Instance.EcsRunHandler.World.GetPool<AddPerkRequest>().Add(requestEntity);
            requestComp.Perk = item;
        }

        disableStatusValue.Value = true;
    }

    protected override void Init()
    {

    }

    protected override void Dispose()
    {
        ActionReroll = null;
        ActionInvokeReward = null;
        _reactivePerks.Dispose();
    }
}

public enum CallbackType { succsess, denied }