using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class ResolveBlockSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DoResolveBlockEvent>, Exc<Dashing>> _filter = default;
        readonly EcsPoolInject<DoResolveBlockEvent> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new ResolveBlockSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var doResolveEvent = ref _pool.Value.Get(entity);
                if(doResolveEvent.SenderPackedEntity.Unpack(_world.Value, out int senderEntity))
                {
                    foreach(var comp in doResolveEvent.Components)
                    {
                        comp.Invoke(entity, senderEntity, _world.Value);
                    }
                }
                
            }
        }
    }
}