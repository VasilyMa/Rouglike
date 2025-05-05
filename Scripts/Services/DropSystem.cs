using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

namespace Client
{
    sealed class DropSystem : MainEcsSystem
    {    ////// TEST 
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<DropEvent>> _filter = default;
        readonly EcsPoolInject<DropEvent> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new DropSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            // add your run code here.
            foreach (var entity in _filter.Value)
            {
                ref var dropEvent = ref _pool.Value.Get(entity);
                var dropItem = dropEvent.dropItem.DropItem(dropEvent.DropPosition, dropEvent.EndPosition);
                if (dropItem is null)
                {
                    _pool.Value.Del(entity);
                    continue;
                }
                if (NavMesh.SamplePosition(dropEvent.EndPosition, out var hit, 5, NavMesh.AllAreas))
                {
                    dropEvent.EndPosition = hit.position;
                }
                dropItem.transform.DOJump(dropEvent.EndPosition, 2, 1, 1f).SetEase(DG.Tweening.Ease.InOutCubic);

                _pool.Value.Del(entity);
            }
        }
    }
}