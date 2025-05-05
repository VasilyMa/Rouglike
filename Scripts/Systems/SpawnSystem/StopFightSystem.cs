using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class StopFightSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<StopFightEvent>> _filter;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<LocalMapComponent> _localMapPool = default;

        public override MainEcsSystem Clone()
        {
            return new StopFightSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                //TODO GENERATION
                //GameState.Instance.ActivePortals();
                ref var localMapComp = ref _localMapPool.Value.Get(BattleState.Instance.GetEntity("LocalMapEntity"));
                localMapComp.CurrentLocalMapPoint.UnlockPoint();
            }
        }
    }
}