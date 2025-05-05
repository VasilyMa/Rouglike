using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class NextRoomPlayerSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<InitUnitEvent, PlayerComponent>> _filter;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;
        readonly EcsFilterInject<Inc<TestGameplayComponent>> _filterTest;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool;
        public override MainEcsSystem Clone()
        {
            return new NextRoomPlayerSystem();
        }
        public override void Run (IEcsSystems systems) 
        {
#if UNITY_EDITOR
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TestScene") return;
#endif
            var state = BattleState.Instance;
            foreach (var entity in _filter.Value)
            {
                //TODO GENERATION
                if(!state.IsMainScene)
                {
                    // var data = SaveModule.DataSave.GetDataByType<GlobalMap>();
                    // if(data != null) data.Dispose();
                    var lobbyTransform = GameObject.Instantiate(ConfigModule.GetConfig<BiomConfig>().Lobby).transform;
                    lobbyTransform.position = Vector3.zero;
                    lobbyTransform.rotation = Quaternion.Euler(0, 45, 0);
                    lobbyTransform.SetParent(state.navMeshSurface.transform);
                    state.UpdateNavMesh();
                    state.PlayerTransfer(lobbyTransform);
                    
                }
                else
                {
                    ref var localMapComp = ref _localMapPool.Value.Get(state.GetEntity("LocalMapEntity"));
                    state.PlayerTransfer(localMapComp.StartTransform);
                }
            }
        }
    }
}