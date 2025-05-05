using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;
namespace Client {
    sealed class TeleportSystem : MainEcsSystem 
    {
        readonly EcsSharedInject<GameState> _state = default;
        private readonly EcsFilterInject<Inc<TeleportComponent>, Exc<DeadComponent>> _filter = default;
        private readonly EcsPoolInject<TeleportComponent> _pool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;

        public override MainEcsSystem Clone()
        {
            return new TeleportSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var transformComp = ref _transformPool.Value.Get(entity);
                ref var teleportComp = ref _pool.Value.Get(entity);
                if (!teleportComp.IsTeleporting)
                {
                    teleportComp.RandomPos = transformComp.Transform.position + Random.insideUnitSphere * teleportComp.TeleportRadiusInRoom;
                }
                if (_navMeshPool.Value.Has(entity))
                {
                    if (!NavMesh.SamplePosition(teleportComp.RandomPos, out NavMeshHit hit, teleportComp.TeleportRadiusInRoom, NavMesh.AllAreas))
                    {
                        teleportComp.RandomPos = transformComp.Transform.position;
                    }
                    else
                    {
                        teleportComp.RandomPos = hit.position;
                        
                    }
                    teleportComp.RandomPos.y = 0;
                    _navMeshPool.Value.Get(entity).NavMeshAgent.Warp(teleportComp.RandomPos);
                }
                teleportComp.RandomPos.y = 0;
                transformComp.Transform.position = teleportComp.RandomPos;

                _pool.Value.Del(entity);
            }
        }
    }
}