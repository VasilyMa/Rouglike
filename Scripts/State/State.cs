using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using Leopotam.EcsLite;

namespace Statement
{
    public abstract class State : MonoBehaviour
    {
        protected static State _instance;
        public static State Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<State>();
                }
                return _instance;
            }
        }
        [SerializeField] protected EcsFeature _ecsFeature;
        protected EcsRunHandler _ecsRunHandler;
        public EcsRunHandler EcsRunHandler => _ecsRunHandler;

        protected Dictionary<string, EcsPackedEntity> _entities = new Dictionary<string, EcsPackedEntity>();
        protected PoolModule pool = new PoolModule();
        private readonly Queue<GameRequest> _requestsGame = new();
        protected IDisposable _processingGameStatus;
        public PlayerInputAction PlayerInputAction;

        private readonly Queue<ShopRequest> _requestsShop = new();
        private IDisposable _processingArsenalStatus;


        protected virtual void Awake()
        {
            _ecsRunHandler = new EcsRunHandler(_ecsFeature);

            _ecsRunHandler.PreInit();

            _processingGameStatus = Observable.EveryUpdate()
                .Where(_ => _requestsGame.Count > 0)
                .Subscribe(_ => ProcessNextGameRequest());


            _processingArsenalStatus = Observable.EveryUpdate()
                .Where(_ => _requestsShop.Count > 0)
                .Subscribe(_ => ProcessNextShopRequest());

            ObserverEntity.Instance.SetNextStatus();
        }

        protected virtual void Start() => _ecsRunHandler.Init();
        protected virtual void Update() => _ecsRunHandler.Run();
        protected virtual void FixedUpdate() => _ecsRunHandler.FixedRun();
        protected virtual void LateUpdate() => _ecsRunHandler.LateRun();
        protected virtual void OnDestroy()
        {
            StopAllCoroutines();
            _processingGameStatus.Dispose();
            _processingArsenalStatus.Dispose();
            ObserverEntity.Instance.ClearTemporary();
            _ecsRunHandler.Destroy();
            PoolModule.Instance.Dispose();
            
        }
        public virtual void RegisterNewEntity(string key, int entity)
        {
            _entities.Add(key, _ecsRunHandler.World.PackEntity(entity));
        } 
        public virtual bool TryGetEntity(string key, out int entity)
        {
            entity = -1;

            if (_entities.TryGetValue(key, out EcsPackedEntity value))
            {
                if(value.Unpack(_ecsRunHandler.World, out int unpackedEntity))
                {
                    entity = unpackedEntity;
                    return true;
                }
            }

            return false;
        }
        public virtual int GetEntity(string key)
        {
            int value = -1;
            if(_entities[key].Unpack(_ecsRunHandler.World, out int entity))
            {
                value = entity;
            }
            return value;
        } 
        public virtual void RemoveEntity(string key)
        {
            if (_entities.ContainsKey(key))
            {
                _entities.Remove(key);
            }
        }
        public virtual Coroutine RunCoroutine(IEnumerator coroutine, Action callback = null)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                
                return null;
            }
        }
        public virtual Coroutine RunCoroutine(IEnumerator coroutine, params Action[] callback)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                
                return null;
            }
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, Action callback = null)
        {
            yield return StartCoroutine(coroutine); // ���� ���������� ���������� ��������

            

            callback?.Invoke(); // �������� ������, ���� �� ����
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, params Action[] callback)
        {
            yield return StartCoroutine(coroutine); // ���� ���������� ���������� ��������

            

            for (int i = 0; i < callback.Length; i++)
            {
                callback[i]?.Invoke();
            }
        }
        public void SendRequest(GameRequest request)
        {
            _requestsGame.Enqueue(request);
        }
        private void ProcessNextGameRequest()
        {
            if (_requestsGame.Count == 0) return;

            GameRequest request = _requestsGame.Dequeue();

            try
            {
                bool shouldExecute = request.Condition?.Invoke() ?? true;

                if (!shouldExecute)
                {
                    
                    request.OnCompleted?.Invoke();
                    return;
                }

                if (request.TargetState.HasValue)
                {
                    Status newRequest = request.TargetState.Value;
                    

                    HandleStateChange(newRequest);
                }

                request.OnCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                
                request.OnCompleted?.Invoke();
            }
        }

        protected virtual void UpdateStatus(Status temporary, Status targetStatus)
        {
            var currentStatuses = ObserverEntity.Instance.OnStatusChange.Value;

            var newStatuses = new Dictionary<Order, Status>(currentStatuses.OnStatusChange.Value)
            {
                [Order.Previous] = currentStatuses.OnStatusChange.Value[Order.Current],
                [Order.Current] = temporary,
                [Order.Temporary] = currentStatuses.OnStatusChange.Value[Order.Current],
                [Order.Next] = targetStatus
            };

            currentStatuses.UpdateStatuses(newStatuses);
        }
        protected virtual void HandleStateChange(Status requestStatus)
        {
            switch (requestStatus)
            {
                case Status.Lobby:
                    UpdateStatus(temporary: Status.Loading, targetStatus: Status.Lobby);
                    SceneManager.LoadScene(2);
                    break;
                case Status.MainMenu:
                    UpdateStatus(temporary: Status.Loading, targetStatus: Status.MainMenu);
                    SceneManager.LoadScene(1);
                    break;
                case Status.Gameplay:
                    UpdateStatus(temporary: Status.Loading, targetStatus: Status.Gameplay);
                    SceneManager.LoadScene(3);
                    break;
            }
        }
        public void SendRequest(ShopRequest request)
        {
            _requestsShop.Enqueue(request);
        }
        private void ProcessNextShopRequest()
        {
            if (_requestsShop.Count == 0) return;

            // ��������� ������ �� �������
            ShopRequest request = _requestsShop.Dequeue();

            try
            {
                // ��������� ������� ���������� (���� �������)
                bool shouldExecute = request.Condition?.Invoke() ?? true;

                if (!shouldExecute)
                {
                    
                    request.OnCompleted?.Invoke();
                    return;
                }

                // ���� ������� ������� ��������� - ������ ���
                if (request.Status.HasValue)
                {
                    

                    HandleArsenalStatusChange(request);
                }

                // �������� ������� ����������
                request.OnCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                
                request.OnCompleted?.Invoke(); // ������ �������� �������, ���� ��� ������
            }
        }
        void HandleArsenalStatusChange(ShopRequest request)
        {
            switch (request.Status)
            {
                case ShopRequestStatus.buyWeapon:
                    PlayerEntity.Instance.RequestBuyWeapon(request.KEY_ID);
                    break;
                case ShopRequestStatus.buyAbility:
                    PlayerEntity.Instance.RequestBuyAbility(request.KEY_ID);
                    break;
                case ShopRequestStatus.selectWeapon:
                    PlayerEntity.Instance.RequestSelectWeapon(request.KEY_ID);
                    break;
                case ShopRequestStatus.selectAbility:
                    PlayerEntity.Instance.RequestSelectAbility(request.KEY_ID);
                    break;
            }
        }
    }
}

public class GameStatus
{
    private ReactiveProperty<Dictionary<Order, Status>> _statusData = new ReactiveProperty<Dictionary<Order, Status>>();
    public IReadOnlyReactiveProperty<Dictionary<Order, Status>> OnStatusChange => _statusData;

    public GameStatus()
    {
        _statusData = new ReactiveProperty<Dictionary<Order, Status>>(
            new Dictionary<Order, Status>
            {
                [Order.Previous] = Status.Init,
                [Order.Current] = Status.none,
                [Order.Temporary] = Status.none,
                [Order.Next] = Status.MainMenu
            });
    }

    public void Subscribe<T>(IReadOnlyReactiveProperty<T> reactiveProperty, Action<T> onChanged)
    {
        reactiveProperty.Subscribe(onChanged).AddTo(ObserverEntity.Instance.MainDisposable);
    }

    public void UpdateStatuses(Dictionary<Order, Status> statuses)
    {
        _statusData.Value = statuses;
    }

    public void SetNext()
    {
        var currentState = _statusData.Value[Order.Current];
        var previousState = _statusData.Value[Order.Previous];
        var temporaryState = _statusData.Value[Order.Temporary];
        var nextState = _statusData.Value[Order.Next];

        _statusData.Value = new Dictionary<Order, Status> { [Order.Previous] = temporaryState, [Order.Current] = nextState, [Order.Temporary] = Status.none, [Order.Next] = Status.none };

        Debug.Log($" <color=cyan>Status Data</color> \n" +
         $"Previous status: {_statusData.Value[Order.Previous]}\n" +
         $"Current status:  {_statusData.Value[Order.Current]}\n" +
         $"Next status:     {_statusData.Value[Order.Next]}\n" +
         "-------------------------");
    }

    public Status GetCurrentStatus()
    {
        return _statusData.Value[Order.Current];
    }
    public Status GetPreviousStatus()
    {
        return _statusData.Value[Order.Previous];
    }


}

public struct GameRequest
{
    public Status? TargetState;
    public Action OnCompleted;
    public Func<bool> Condition;

    public GameRequest(Status status, Action onComplete = null, Func<bool> condition = null)
    {
        TargetState = status;
        OnCompleted = onComplete;
        Condition = condition;
    }
}
public enum Order { Previous, Temporary, Current, Next}
public enum Status { none, Init, MainMenu, Gameplay, Overlay, Loading, Cutscene, Lobby, Lose, EndLevel, EndChapter, BossFight,EndGame }

public struct ShopRequest
{
    public ShopRequestStatus? Status;
    public string KEY_ID;
    public Action OnCompleted;
    public Func<bool> Condition;

    public ShopRequest(string key_id, ShopRequestStatus status, Action onComplete = null, Func<bool> condition = null)
    {
        KEY_ID = key_id;
        Status = status;
        OnCompleted = onComplete;
        Condition = condition;
    }
}
public enum ShopRequestStatus { buyWeapon, buyAbility, selectWeapon, selectAbility }