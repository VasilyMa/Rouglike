using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RelicDropSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<MomentDeadEvent,DropComponent>, Exc<PlayerComponent>> _filter;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<DropEvent> _dropEvent;
        readonly EcsPoolInject<DropComponent> _dropPool;

        public override MainEcsSystem Clone()
        {
            return new RelicDropSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var dropComp = ref _dropPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                if (dropComp.dropConfig is null) continue;
                var dropList = dropComp.dropConfig.GetDropLoot();
                foreach (var dropItem in dropList)
                {
                    ref var dropEvent = ref _dropEvent.Value.Add(_world.Value.NewEntity());
                    dropEvent.DropPosition = transformComp.Transform.position;
                    Vector3 endPos = transformComp.Transform.position + Random.insideUnitSphere * 1.75f;
                    dropEvent.EndPosition = endPos;
                    dropEvent.dropItem = dropItem;
                }
            }
        }
    }
}