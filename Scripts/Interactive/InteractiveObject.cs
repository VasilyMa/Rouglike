using System;
using System.Collections;
using System.Collections.Generic;

using Statement;
using Client;
using UniRx;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI;
using Leopotam.EcsLite;

public abstract class InteractiveObject : MonoBehaviour
{
    [ReadOnlyInspector] public Transform _target;
    protected InteractiveObserver observer;
    [Header("Distance value to activate")] public float DistanceToActive;

    protected ReactiveProperty<bool> _priorityValue = new ReactiveProperty<bool>(false);  
    protected ReactiveProperty<Vector2> positionValue = new ReactiveProperty<Vector2>();
    protected ReactiveProperty<InteractiveObjectStatus> interactiveObjectStatusValue = new ReactiveProperty<InteractiveObjectStatus>();
    protected ReactiveProperty<bool> disableStatusValue = new ReactiveProperty<bool>();
    protected ReactiveProperty<float> distanceValue = new ReactiveProperty<float>();
    protected Subject<KeyCode> onInputButtonSubject = new Subject<KeyCode>();
    private bool _isActuallyActive = false;
    [Header("Use the HotKey")] public KeyCode UseKeyCode = KeyCode.F;

    protected CompositeDisposable disposables = new CompositeDisposable();

    // Public properties
    public IReadOnlyReactiveProperty<bool> OnPriorityChange => _priorityValue;
    public IReadOnlyReactiveProperty<Vector2> OnScreenPositionChange => positionValue;
    public IReadOnlyReactiveProperty<InteractiveObjectStatus> OnInteractiveStatusValueChange => interactiveObjectStatusValue;
    public IReadOnlyReactiveProperty<bool> OnDisableStatusValueChange => disableStatusValue;
    public IReadOnlyReactiveProperty<float> OnDistanceValueChange => distanceValue;
    public IObservable<KeyCode> OnButtonPressed => onInputButtonSubject;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        

        observer = new InteractiveObserver(this);// ObserverEntity.Instance.GetNewInteractiveObserver(); new InteractiveObserver(this);

        // Подписка на нажатие кнопки
        Observable.EveryUpdate()
            .Where(_ => _target != null && _priorityValue.Value && Input.GetKeyDown(UseKeyCode))
            .Subscribe(_ => InputButton())
            .AddTo(disposables);

        Observable.EveryUpdate()
            .Subscribe(_ => UpdatePosition())
            .AddTo(disposables);

        ObserverEntity.Instance.AddInteractive(observer);
        OnInteractiveStatusValueChange.Subscribe(SwitchCurrentActivity);
    }
    protected virtual void OnDisable()
    {
        interactiveObjectStatusValue.Value = InteractiveObjectStatus.far;
        _priorityValue.Value = false;

        disposables.Clear();
        onInputButtonSubject.OnCompleted();

        ObserverEntity.Instance.RemoveInteractive(observer);

        Dispose();
    }
    protected virtual void UpdatePosition()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        positionValue.Value = screenPosition;
    }
    protected virtual void InputButton()
    {
        onInputButtonSubject.OnNext(UseKeyCode);

        if (disableStatusValue.Value) return;

        switch (interactiveObjectStatusValue.Value)
        {
            case InteractiveObjectStatus.far:
                interactiveObjectStatusValue.Value = InteractiveObjectStatus.far;
                break;
            case InteractiveObjectStatus.near:
                interactiveObjectStatusValue.Value = InteractiveObjectStatus.active;
                _isActuallyActive = true;
                break;
            case InteractiveObjectStatus.active:
                interactiveObjectStatusValue.Value = InteractiveObjectStatus.near;
                break;
        } 
    }
    private void SwitchCurrentActivity(InteractiveObjectStatus status)
    {
        if (_isActuallyActive && status != InteractiveObjectStatus.active)
        {
            _isActuallyActive = false;
            var Pool = State.Instance.EcsRunHandler.World.GetPool<RequestSwithControllerEvent>();
            var playerEntity = State.Instance.GetEntity("PlayerEntity");
            if (!Pool.Has(playerEntity)) Pool.Add(playerEntity).InputActionPreset = InputActionPreset.FullControl;
            else
            {
                var comp = Pool.Get(playerEntity).InputActionPreset = InputActionPreset.FullControl;
            }
        }
    }
    public virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        _target = collider.transform;

        ObserverEntity.Instance.UpdateEveryInteractive();

        Init();
    }
    public virtual void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        _target = null;
        _priorityValue.Value = false;

        ObserverEntity.Instance.UpdateEveryInteractive();
    }
    public virtual void Subscribe<T>(IObservable<T> observable, Action<T> onNext)
    {
        observable.Subscribe(onNext).AddTo(disposables);
    }
    protected abstract void Init();
    protected abstract void Dispose();
    public float CheckDistance()
    {
        if (_target == null) return float.MaxValue;

        return Vector3.Distance(transform.position, _target.position);
    }
    public void SetPriority(bool value)
    {
        if (value)
        {
            if (interactiveObjectStatusValue.Value != InteractiveObjectStatus.active)
            {
                _priorityValue.Value = true;
                interactiveObjectStatusValue.Value = InteractiveObjectStatus.near;
            }
        }
        else
        {
            if (_priorityValue.Value)
            {
                var state = BattleState.Instance;
                var world = state.EcsRunHandler.World;
                var playerEntity = state.GetEntity("PlayerEntity");

                if (world.GetPool<InteractWithObjectComponent>().Has(playerEntity))
                {
                    world.GetPool<InteractWithObjectComponent>().Del(playerEntity);
                }
                
            }

            _priorityValue.Value = false;
            interactiveObjectStatusValue.Value = InteractiveObjectStatus.far;
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;

        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToActive);

        GUIStyle boldStyle = new GUIStyle();
        boldStyle.normal.textColor = Color.black;
        boldStyle.fontStyle = FontStyle.Bold;
        boldStyle.fontSize = 25;

        GUIStyle shadowStyle = new GUIStyle(boldStyle);
        shadowStyle.normal.textColor = new Color(0, 0, 0, 0.5f);

        Vector3 textPosition = transform.position + Vector3.up * 0.5f;

        /*Handles.Label(textPosition + Vector3.right * 0.1f + Vector3.down * 0.1f,
                    $"Activation Radius: {DistanceToActive}m", shadowStyle);*/

        Handles.Label(textPosition,
                    $"Activation Radius: {DistanceToActive}m", boldStyle);
    }
#endif

}

public enum InteractiveObjectStatus { far, near, active }