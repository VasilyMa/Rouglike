using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Client;

using Unity.AI.Navigation;

using UnityEngine;
using UnityEngine.VFX;
using UniRx;
using UnityEngine.SceneManagement;
using FMOD.Studio;

namespace Statement
{
    public class BattleState : State
    {
        public static new BattleState Instance
        {
            get
            {
                return (BattleState)State.Instance;
            }
        }

        public int IndexWave = -1;
        public RoomMB CurrentRoom;
        public bool IsMainScene;
        public NavMeshSurface navMeshSurface;

        private static float _defaultFixedDT = 0.02f;
        private static float _defaultVFXDT = 0.01666667f;
        private bool isPause;

        public float currentMatchTick = 0f;
        public float currentDamage;
        public int kills;
        protected override void Awake()
        {
            base.Awake();
            this.navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
            ResetTime();
        }
        protected override void Start()
        {
            base.Start();

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => InvokePause())
                .AddTo(this);

            Observable.EveryUpdate()
                .Where(_ => ObserverEntity.Instance.OnInteractiveChange.Count > 0) // Добавлено лямбда-выражение
                .Subscribe(_ => ObserverEntity.Instance.UpdateEveryInteractive())
                .AddTo(this);

            // currentMatchTick = 0f;

            Observable.EveryUpdate()
                .Subscribe(_ => MatchTick())
                .AddTo(this);

            ////
            SaveModule.SaveSingleData<SlotDataContainer>();
            ////
            ObserverEntity.Instance.SubscribeToList(ObserverEntity.Instance.OnInteractiveChange, CancelInteractive);
        }

        void MatchTick() => currentMatchTick += Time.deltaTime; 
        public void AddKill() => kills++;
        public void AddDamage(float value)
        {
            currentDamage += value;
        }
        protected override void OnDestroy()
        {
            if (TryGetEntity("PlayerEntity", out int entity))
            {
                if (_ecsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(entity))
                {
                    _ecsRunHandler.World.GetPool<InteractWithObjectComponent>().Del(entity);
                }
            }
           base.OnDestroy();
        }
      
        void CancelInteractive(InteractiveObserver interactiveObserver, bool value)
        {
            if (IsMainScene)
            {
                if (!value)
                {
                    if (ObserverEntity.Instance.OnInteractiveChange.Count == 0)
                    {
                        if (TryGetEntity("PlayerEntity", out int entity))
                        {
                            if (_ecsRunHandler.World.GetPool<InteractWithObjectComponent>().Has(entity))
                            {
                                _ecsRunHandler.World.GetPool<InteractWithObjectComponent>().Del(entity);
                            }
                        }
                    }
                }
            }
        }
        void InvokePause()
        {
            

            if (isPause)
            {
                if (IsMainScene)
                {
                    SendRequest(new GameRequest(Status.Gameplay));
                    ResetTime();
                }
                else
                {
                        SendRequest(new GameRequest(Status.Lobby));
                }
                isPause = false;
            }
            else
            {
                if(IsMainScene)
                    Time.timeScale = 0f;

                SendRequest(new GameRequest(Status.Overlay));
                isPause = true;
            }
        }

        public void PlayerTransfer(Transform point)
        {
            //todo ������� ������� �������, �������� ����������
            var world = EcsRunHandler.World;

            EcsPool<TransformComponent> _transformPool = world.GetPool<TransformComponent>();
            EcsPool<NavMeshComponent> _navMeshPool = world.GetPool<NavMeshComponent>();
            ref var navMeshComp = ref _navMeshPool.Get(GetEntity("PlayerEntity"));
            ref var transfromComp = ref _transformPool.Get(GetEntity("PlayerEntity"));
            transfromComp.Transform.position = point.position - point.transform.forward * 10;
            navMeshComp.NavMeshAgent.Warp(point.position - point.transform.forward * 10);
        }
        public void UpdateGlobalMap(Vector2Int position)
        {
            ref var globalMapComp = ref EcsRunHandler.World.GetPool<GlobalMapComponent>().Get(GetEntity("GlobalMapEntity"));
            GlobalMapPoint currentPoint = globalMapComp.PointsArray[position.x, position.y];

            GlobalMapPoint lastPoint = globalMapComp.PointsArray[globalMapComp.CurrentGlobalMapPointPosition.x, globalMapComp.CurrentGlobalMapPointPosition.y];
            lastPoint.PointState = PointStates.Completed;
            foreach (var link in lastPoint.ExitList)
            {
                if (link != currentPoint)
                {
                    link.PointState = PointStates.Closed;
                }
            }
            currentPoint.PointState = PointStates.ThisRoom;
            globalMapComp.CurrentGlobalMapPointPosition = position;
            foreach (var link in currentPoint.ExitList)
            {
                link.PointState = PointStates.Open;
            }

            PlayerEntity.Instance.Map.ProcessUpdataData();
        }
        public GlobalMapPoint[,] UpdateGlobalMapForUI(int index)
        {
            
            ref var globalMapComp = ref EcsRunHandler.World.GetPool<GlobalMapComponent>().Get(GetEntity("GlobalMapEntity"));
            if(index >= globalMapComp.BiomCount) index = globalMapComp.BiomCount - 1;
            var result = new GlobalMapPoint[globalMapComp.MaxWidth, globalMapComp.BiomeLenLength];
            List<GlobalMapPoint> serviceCont = new List<GlobalMapPoint>();
            for (int i = 0; i < globalMapComp.MaxWidth; i++)
            {
                for (int j = 0; j < globalMapComp.BiomeLenLength; j++)
                {
                    result[i, j] = globalMapComp.PointsArray[i, j + index * globalMapComp.BiomeLenLength + 1];
                    result[i, j].ForUIPosition = new Vector2Int(i, j);
                    serviceCont.Add(result[i, j]);
                }
            }
            for (int i = 0; i < globalMapComp.MaxWidth; i++)
            {
                for (int j = 0; j < globalMapComp.BiomeLenLength; j++)
                {
                    List<GlobalMapPoint> serviceExitList = new List<GlobalMapPoint>();
                    List<GlobalMapPoint> serviceEnterList = new List<GlobalMapPoint>();
                    foreach(var exit in result[i, j].ExitList)
                    {
                        if(serviceCont.Contains(exit))
                        {
                            serviceExitList.Add(exit);
                        }
                    }
                    foreach (var enter in result[i,j].EnterList)
                    {
                        if(serviceCont.Contains(enter))
                        {
                            serviceExitList.Add(enter);
                        }
                    }

                    result[i, j].ForUIEnterList = serviceEnterList;
                    result[i, j].ForUIExitList = serviceExitList;
                }
                
            }
            return result;

        }
        public void UpdateNavMesh()
        {
            //navMeshSurface.RemoveData(navMeshSurface.navMeshData);
            navMeshSurface.BuildNavMesh();
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        }

        public void ResetTime()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _defaultFixedDT;
            VFXManager.fixedTimeStep = _defaultVFXDT;
        }
        protected override void HandleStateChange(Status requestStatus)
        {
            

            switch (requestStatus)
            {
                case Status.Lobby:
                    UpdateStatus(temporary: Status.Loading, requestStatus);
                    SceneManager.LoadScene(2);
                    SoundEntity.Instance.PlayAmbientAttached(SoundEntity.Instance.GetSoundConfig()._lobbyAmbient);
                    SaveModule.SaveSingleData<PlayerData>();
                    SaveModule.SaveSingleData<SlotDataContainer>();
                    PlayerEntity.Instance.Reset();
                    break;
                case Status.MainMenu:
                    UpdateStatus(temporary: Status.Loading, requestStatus);
                    SceneManager.LoadScene(1);
                    SoundEntity.Instance.PlayAmbientAttached(SoundEntity.Instance.GetSoundConfig()._mainMenuAmbient);
                    break;
                case Status.Gameplay:
                    UpdateGameplayStatus(requestStatus);
                    break;
                case Status.Overlay:
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    break;
                case Status.BossFight:
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    break;
                case Status.EndLevel:
                    ObserverEntity.Instance.ChangeGameResultValue(new GameResult(currentMatchTick, kills, currentDamage));
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    SaveModule.SaveSingleData<PlayerData>();
            SaveModule.SaveSingleData<SlotDataContainer>();
                    break;
                case Status.EndChapter:
                    ObserverEntity.Instance.ChangeGameResultValue(new GameResult(currentMatchTick, kills, currentDamage));
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    SaveModule.SaveSingleData<PlayerData>();
                    PlayerEntity.Instance.Reset();
            SaveModule.SaveSingleData<SlotDataContainer>();
                    break;
                case Status.Lose:
                    ObserverEntity.Instance.ChangeGameResultValue(new GameResult(currentMatchTick, kills, currentDamage));
                    PlayerEntity.Instance.Reset();
                    SaveModule.SaveSingleData<PlayerData>();
            SaveModule.SaveSingleData<SlotDataContainer>();
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    //Time.timeScale = 0f;
                    break;
                case Status.EndGame:
                    UpdateStatus(temporary: Status.none, requestStatus);
                    ObserverEntity.Instance.SetNextStatus();
                    if (!EcsRunHandler.World.GetPool<CreateGlobalMapSelfRequest>().Has(GetEntity("GlobalMapEntity")))
                    EcsRunHandler.World.GetPool<CreateGlobalMapSelfRequest>().Add(GetEntity("GlobalMapEntity"));
                    SaveModule.SaveSingleData<PlayerData>();
            SaveModule.SaveSingleData<SlotDataContainer>();
                    break;
            }
        }

        void UpdateGameplayStatus(Status status)
        {
            var previousStatus = ObserverEntity.Instance.OnStatusChange.Value.OnStatusChange.Value[Order.Previous];
            var currentStatus = ObserverEntity.Instance.OnStatusChange.Value.OnStatusChange.Value[Order.Current];

            switch (currentStatus)
            {
                case Status.Lobby:
                    UpdateStatus(temporary: Status.Loading, targetStatus: status);
                    SceneManager.LoadScene(3);
                    return;
                case Status.EndLevel:
                    UpdateStatus(temporary: Status.Loading, targetStatus: status);
                    SceneManager.LoadScene(3);
                    return;
                case Status.EndChapter:
                    UpdateStatus(temporary: Status.Loading, targetStatus: status);
                    SceneManager.LoadScene(3);
                    return;
            }

            UpdateStatus(temporary: Status.none, status);
            ObserverEntity.Instance.SetNextStatus();
        }
    }
}