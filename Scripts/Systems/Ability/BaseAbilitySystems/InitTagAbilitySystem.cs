using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitTagAbilitySystem : MainEcsSystem {   
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitAbilityEvent, AbilityComponent, PlayerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
         public override MainEcsSystem Clone()
        {
            return new InitTagAbilitySystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                
            }
        }
    }
}