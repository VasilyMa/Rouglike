using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SpawnAbilitySystem : MainEcsSystem 
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SpawnAbilityEvent>> _filter = default;
        readonly EcsPoolInject<SpawnAbilityEvent> _pool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;

        public override MainEcsSystem Clone()
        {
            return new SpawnAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in  _filter.Value) 
            {
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);
                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                    if (abilityComp.Ability.SourceAbility.AbilityType == AbilitySystem.AbilityTypes.Spawn)
                    {
                        _abilityPressedPool.Value.Add(abilityEntity);
                        _pool.Value.Del(entity);
                    }
                }
                
            }
        }
    }
}