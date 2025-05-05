using AbilitySystem;
using Client;

using DG.Tweening;

using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using Statement;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

using static UnityEngine.EventSystems.EventTrigger;

public class InteractiveOrbObject : InteractiveObject, IPool
{
    public string UniqueID;

    public InputActionReference InputE;
    public InputActionReference InputQ;

    [Header("Dissolve the HotKey")] public KeyCode DissolveKeyCode = KeyCode.G;
    private Subject<KeyCode> onInputButtonDissolve = new Subject<KeyCode>();
    public IObservable<KeyCode> OnButtonDissolvePressed => onInputButtonDissolve;

    [Header("Replace Q the HotKey")] public KeyCode ReplaceQAbilityKeyCode = KeyCode.Q;
    private Subject<KeyCode> onInputButtonReplaceQAbilitySubject = new Subject<KeyCode>();
    public IObservable<KeyCode> OnButtonReplaceQAbilityPressed => onInputButtonReplaceQAbilitySubject;

    [Header("Replace E the HotKey")] public KeyCode ReplaceEAbilityKeyCode = KeyCode.E;
    private Subject<KeyCode> onInputButtonReplaceEAbilitySubject = new Subject<KeyCode>();
    public IObservable<KeyCode> OnButtonReplaceEAbilityPressed => onInputButtonReplaceEAbilitySubject;

    public Action ActionDissolve;
    public Action ActionReplaceE;
    public Action ActionReplaceQ;

    [SerializeReference][ShowIf("replaceOrb")] public IDrop replaceOrb;

    private ReactiveProperty<OrbResource> _orbValue = new ReactiveProperty<OrbResource>();
    public IReactiveProperty<OrbResource> OnOrb => _orbValue;
    private ReactiveProperty<AbilityBase> _currentAbility = new ReactiveProperty<AbilityBase>();
    public IReactiveProperty<AbilityBase> OnAbilityChange => _currentAbility;

    private AbilityBase _temporaryAbilityBase;
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

    protected override void OnEnable()
    {
        base.OnEnable();

        Observable.EveryUpdate()
             .Where(_ => _priorityValue.Value)
             .Where(_ => _target != null)
             .Where(_ => Input.GetKeyDown(ReplaceEAbilityKeyCode))
            .Subscribe(_ =>
            {
                ReplaceE();
            })
            .AddTo(disposables);

        Observable.EveryUpdate()
             .Where(_ => _priorityValue.Value)
             .Where(_ => _target != null)
             .Where(_ => Input.GetKeyDown(ReplaceQAbilityKeyCode))
            .Subscribe(_ =>
            {
                ReplaceQ();
            })
            .AddTo(disposables);

        Observable.EveryUpdate()
             .Where(_ => _priorityValue.Value)
             .Where(_ => _target != null)
             .Where(_ => Input.GetKeyDown(DissolveKeyCode))
            .Subscribe(_ =>
            {
                Dissolve();
            })
            .AddTo(disposables);

        ActionDissolve = Dissolve;
        ActionReplaceE = ReplaceE;
        ActionReplaceQ = ReplaceQ;

        Subscribe(OnInteractiveStatusValueChange, onStatusChange);
    }

    void onStatusChange(InteractiveObjectStatus status)
    {
        

        switch (status)
        {
            case InteractiveObjectStatus.far:
                DelInteractiveComponent();
                break;
            case InteractiveObjectStatus.near:
                DelInteractiveComponent();
                break;
            case InteractiveObjectStatus.active:
                AddInteractiveComponent();
                break;
        }
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

    void ReplaceE()
    {
        if (!CheckInteractive()) return;

        var inputReference = InputE.action.name;

        SaveAbilityData(inputReference);
        Replace(InputE);
        AdditionalDrop();
    }
    void ReplaceQ()
    {
        if (!CheckInteractive()) return;

        var inputReference = InputQ.action.name;

        SaveAbilityData(inputReference);
        Replace(InputQ);
        AdditionalDrop();
    }

    void Replace(InputActionReference reference)
    {
        var state = State.Instance;
        var playerEntity = state.GetEntity("PlayerEntity");

        var newEntity = state.EcsRunHandler.World.NewEntity();

        ref var initAbilityComp = ref state.EcsRunHandler.World.GetPool<InitAbilityEvent>().Add(newEntity);

        initAbilityComp.AbilityBase = _currentAbility.Value;
        initAbilityComp.IsReplace = true;
        initAbilityComp.NewAbilityInputReference = reference;
        initAbilityComp.PackedEntity = state.EcsRunHandler.World.PackEntity(playerEntity);

        foreach (var inputBlock in initAbilityComp.AbilityBase.SourceAbility.InputBlocks)
        {
            for (int i = 0; i < inputBlock.Components.Count; i++)
            {
                if (inputBlock.Components[i] is RequestChangeAnimationEvent requestChangeAnimationEvent)
                {
                    if (requestChangeAnimationEvent.IsUniqueAnimation)
                    {
                        // ������ ����� ��������� � ����������� ����������
                        RequestChangeAnimationEvent updatedEvent = requestChangeAnimationEvent;

                        UpdateAnimationEvent(ref updatedEvent, reference);

                        // ��������� ������� � ������������ ������
                        inputBlock.Components[i] = updatedEvent;
                    }
                }
            }
        }
        foreach (var timeLineBlock in initAbilityComp.AbilityBase.SourceAbility.TimeLineBlocks)
        {
            for (int i = 0; i < timeLineBlock.FXComponents.Count; i++)
            {
                if (timeLineBlock.FXComponents[i] is RequestChangeAnimationEvent requestChangeAnimationEvent)
                {
                    if (requestChangeAnimationEvent.IsUniqueAnimation)
                    {
                        // ������ ����� ��������� � ����������� ����������
                        RequestChangeAnimationEvent updatedEvent = requestChangeAnimationEvent;

                        UpdateAnimationEvent(ref updatedEvent, reference);

                        // ��������� ������� � ������������ ������
                        timeLineBlock.FXComponents[i] = updatedEvent;
                    }
                }
            }
        }
    }
    void UpdateAnimationEvent(ref RequestChangeAnimationEvent request, InputActionReference inputReference)
    {
        if (inputReference == InputQ)
            request.AnimationType = AnimationTypes.CombatSlot1;
        if (inputReference == InputE)
            request.AnimationType = AnimationTypes.CombatSlot2;
    }
    void SaveAbilityData(string reference)
    {
        var state = State.Instance;
        var world = state.EcsRunHandler.World;
        var playerEntity = state.GetEntity("PlayerEntity");

        ref var abilityUnitMB = ref world.GetPool<AbilityUnitComponent>().Get(playerEntity).AbilityUnitMB;

        if (abilityUnitMB.GetAbilitiesListByActionName(reference).Unpack(world, out var abilityEntity))
        {
            ref var AbilityComp = ref world.GetPool<AbilityComponent>().Get(abilityEntity);
            _temporaryAbilityBase = AbilityComp.Ability;
        }
    }

    void Dissolve()
    {
        if (!CheckInteractive()) return;

        _orbValue.Value.Dissolve();

        ReturnToPool();
    }

    public void InitPool()
    {

    }
    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }
    protected override void Dispose()
    {
        
        onInputButtonDissolve.OnCompleted();
        onInputButtonReplaceEAbilitySubject.OnCompleted();
        onInputButtonReplaceQAbilitySubject.OnCompleted();
        ActionDissolve = null;
        ActionReplaceE = null;
        ActionReplaceQ = null;
    }

    protected override void Init()
    {
    }
    public void SetValue(OrbResource orbValue)
    {
        _currentAbility.Value = orbValue.GetRandomAbility();
        _orbValue.Value = orbValue;
    }

    bool CheckInteractive()
    {
        var state = State.Instance;
        var playerEntity = state.GetEntity("PlayerEntity");

        if (state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(playerEntity)) return true;

        return false;
    }

    void AdditionalDrop()
    {
        var state = State.Instance;
        var world = state.EcsRunHandler.World;
        var playerEntity = state.GetEntity("PlayerEntity");

        ref var transformComp = ref world.GetPool<TransformComponent>().Get(playerEntity);

        Vector3 endPos = transformComp.Transform.position + UnityEngine.Random.insideUnitSphere * 1.75f;

        if (NavMesh.SamplePosition(endPos, out var hit, 5, NavMesh.AllAreas))
        {
            endPos = hit.position;
        }

        if (state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(playerEntity))
        {
            state.EcsRunHandler.World.GetPool<InteractWithObjectComponent>().Del(playerEntity);
        }

        _priorityValue.Value = false;

        interactiveObjectStatusValue.Value = InteractiveObjectStatus.near;

        transform.position = transformComp.Transform.position + Vector3.up;

        _currentAbility.Value = _temporaryAbilityBase;

        _temporaryAbilityBase = null;

        transform.DOJump(endPos, 2, 1, 1f).SetEase(DG.Tweening.Ease.InOutCubic).OnComplete(onComplete);
    }

    void onComplete()
    {
        ObserverEntity.Instance.UpdateEveryInteractive();
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
