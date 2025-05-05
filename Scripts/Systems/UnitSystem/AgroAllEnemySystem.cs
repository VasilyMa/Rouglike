using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class AgroAllEnemySystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsFilterInject<Inc<PlayerComponent>> _playerFIlter = default;
        readonly EcsFilterInject<Inc<EnemyComponent>, Exc<TargetComponent, DeadComponent>> _enemyFilter = default;
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent>, Exc<DeadComponent>> _filter = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var player in _playerFIlter.Value)
            {
                if(_filter.Value.GetEntitiesCount() == 0)
                {
                    foreach (var entity in _enemyFilter.Value)
                    {
                        ref var targetComp = ref _targetPool.Value.Add(entity);
                        targetComp.TargetPackedEntity = State.Instance.EcsRunHandler.World.PackEntity(player);
                    }
                }
            }
        }
    }
}