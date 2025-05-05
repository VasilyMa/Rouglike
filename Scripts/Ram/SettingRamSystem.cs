using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    sealed class SettingRamSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<RamComponent>> _filter;
        readonly EcsPoolInject<RamComponent> _ramPool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<TargetComponent> _targetPool;
        readonly EcsPoolInject<MoveEvent> _moveEventPool;
        public void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var transformEnemy = ref _transformPool.Value.Get(entity);
                ref var targetComp = ref _targetPool.Value.Get(entity);
                ref var ramComp = ref _ramPool.Value.Get(entity);

                int entityTarget = State.Instance.GetEntity("PlayerEntity");
                ref var transformTarget = ref _transformPool.Value.Get(entityTarget);
                Vector3 directionToPlayer = (transformTarget.Transform.position - transformEnemy.Transform.position).normalized;
                Vector3 distanceToPlayer = directionToPlayer * ramComp.Distance;
                int layerMaskOnlyObstacle = 1 << 8;
                RaycastHit hit;
                if (Physics.Raycast(transformEnemy.Transform.position, directionToPlayer, out hit, ramComp.Distance, layerMaskOnlyObstacle))
                {
                    distanceToPlayer = hit.distance*distanceToPlayer.normalized;
                }
                Vector3 targetPosition = transformEnemy.Transform.position + distanceToPlayer;
                transformEnemy.Transform.LookAt(transformTarget.Transform.position);
                if (!_moveEventPool.Value.Has(entity)) _moveEventPool.Value.Add(entity);
                ref var moveEvent = ref _moveEventPool.Value.Get(entity);
                moveEvent.TimeMove = distanceToPlayer.magnitude / ramComp.SpeedMove;
                moveEvent.Boost = ramComp.Boost;
                moveEvent.EndPoint = targetPosition;
            }
        }
    }
}