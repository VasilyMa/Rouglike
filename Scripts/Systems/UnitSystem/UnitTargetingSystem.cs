using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class UnitTargetingSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<EnemyComponent/*, AIComponent*/>, Exc<TargetComponent, AwakeningComponent, DeadComponent>> _filter = default;
        
        readonly EcsFilterInject<Inc<PlayerComponent, TransformComponent>> _playerFilter = default;
        readonly EcsPoolInject<TransformComponent> _transformComponent = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;

        public override MainEcsSystem Clone()
        {
            return new UnitTargetingSystem();
        }

        public override void Run (IEcsSystems systems) {
            /*foreach(var player in _playerFilter.Value)
            {
                
                foreach(var entity in _filter.Value)
                {
                    ref var playerView = ref _transformComponent.Value.Get(player);
                    ref var viewComp = ref _transformComponent.Value.Get(entity);
                    ref var AIComp = ref _AIPool.Value.Get(entity);

                    float distance = Vector3.Distance(playerView.Transform.position, viewComp.Transform.position);
                    if(distance <= AIComp.AgroDistance)
                    {
                        SetTarget(entity, player);
                    }
                }
            }*/
        }
        public void SetTarget(int entity, int player)
        {
            ref var targetComp = ref _targetPool.Value.Add(entity);

            targetComp.TargetPackedEntity = State.Instance.EcsRunHandler.World.PackEntity(player);
        }
    }
}