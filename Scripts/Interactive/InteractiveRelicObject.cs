using Client;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Statement;
using UniRx;
using System.Xml;

public class InteractiveRelicObject : InteractiveObject, IPool
{
    public string UniqueID;

    [Header("Dissolve the HotKey")] public KeyCode DissolveKeyCode = KeyCode.G;
    private Subject<KeyCode> onInputButtonDissolve = new Subject<KeyCode>();
    public IObservable<KeyCode> OnButtonDissolvePressed => onInputButtonDissolve;

    private ReactiveProperty<RelicResource> _relicValue = new ReactiveProperty<RelicResource>();
    public IReadOnlyReactiveProperty<RelicResource> OnRelicValueChange => _relicValue;

    public Action ActionDissolve;
    public Action ActionEquip;

    
    #region pool
    protected bool isAvaiable;
    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;
    public string KEY_ID;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        UniqueID = Guid.NewGuid().ToString();
    }

    public void SetData(RelicResource relic)
    {
        _relicValue.Value = relic;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        Observable.EveryUpdate()
             .Where(_ => _target != null)
             .Where(_ => Input.GetKeyDown(DissolveKeyCode))
            .Subscribe(_ =>
            {
                InputDissolve();
            })
            .AddTo(disposables);
    }
    void AddInteractiveComponent()
    {
        var state = State.Instance;
        var playerEntity = State.Instance.GetEntity("PlayerEntity");

        if (!state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(playerEntity))
        {
            state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Add(playerEntity);
            
        }
    }

    void DelInteractiveComponent()
    {
        if (!_priorityValue.Value) return;

        var state = State.Instance;
        var playerEntity = State.Instance.GetEntity("PlayerEntity");

        if (state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(playerEntity))
        {
            state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Del(playerEntity);
            

        }
    }

    protected override void InputButton()
    {
        base.InputButton();

        ActionEquip?.Invoke();

        ReturnToPool();
    }

    protected void InputDissolve()
    {
        ActionDissolve?.Invoke();

        ReturnToPool();
    }

    protected override void Init()
    {
        ActionDissolve = _relicValue.Value.SourceRelic.Dissolve;
        //ActionEquip = _relicValue.Value.SourceRelic.InvokeRelic;
        ActionEquip = PickupAndAddRelicToTemporaryCollection;

        ActionDissolve += ReturnToPool;
        ActionEquip += ReturnToPool;
    }
    private void PickupAndAddRelicToTemporaryCollection()
    {
        _relicValue.Value.SourceRelic.InvokeRelic();
        PlayerEntity.Instance.RelicCollectionData.AddTemporaryRelic(new CurrentRelicData(_relicValue.Value.KEY_ID));
    }

    protected override void Dispose()
    {
        ActionDissolve = null;
        ActionEquip = null;
    }

    public void Update()
    {
        this.transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    public void InitPool()
    {
    }
    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }
#endif
}
