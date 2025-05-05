using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;
namespace Client {
    sealed class TemporaryCirclingCheckSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<TargetComponent, AbilityUnitComponent>, Exc<Circling, DeadComponent>> _unitFilter = default;
        readonly private EcsPoolInject<AbilityUnitComponent> _abilityPool = default;
        readonly private EcsPoolInject<Circling> _circlingPool = default;
        readonly private EcsPoolInject<CooldownRecalculationComponent> _cooldownPool = default;

        public override MainEcsSystem Clone()
        {
            return new TemporaryCirclingCheckSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _unitFilter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                var entities = abilityComp.AbilityUnitMB.GetAllAbilitiesEntities();
                bool hasSomethingToCast = false;
                foreach (int abilityEntity in entities)
                {
                    if (!_cooldownPool.Value.Has(abilityEntity)) hasSomethingToCast = true;
                }
                if (hasSomethingToCast) continue;
                _circlingPool.Value.Add(entity);
            }
        }
    }
}