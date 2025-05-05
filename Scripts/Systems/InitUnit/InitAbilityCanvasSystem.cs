using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UI;
using Statement;

namespace Client {
    sealed class InitAbilityCanvasSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<PlayerComponent, InitUnitEvent>> _filter;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitAbilityCanvasSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);
                ref var interfaceComp = ref _interfacePool.Value.Get(State.Instance.GetEntity("InterfaceEntity"));
                ref var transformComp = ref _transformPool.Value.Get(State.Instance.GetEntity("PlayerEntity"));

            }
        }
    }
}