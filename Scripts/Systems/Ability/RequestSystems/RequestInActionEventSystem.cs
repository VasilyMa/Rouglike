using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RequestInActionEventSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestInActionEvent>> _filter = default;
        readonly EcsPoolInject<RequestInActionEvent> _requestPool = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestInActionEventSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                if(requestComp.TargetPackedEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_inActionPool.Value.Has(targetEntity))
                    {
                        _inActionPool.Value.Add(targetEntity);
                    }
                }
            }
        }
    }
}