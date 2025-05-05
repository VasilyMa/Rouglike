using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    /// <summary>
    /// System updates DefenseContext that holds info about defense abilities
    /// Iterates through all entities with UnitBrain and DefenseContext components
    /// </summary>
    sealed class UpdateDefenseContextSystem : MainEcsSystem
    {
        private readonly EcsFilterInject<Inc<UnitBrain, DefenseContext>> _filter = default;
        private readonly EcsPoolInject<DefenseContext> _defenseContextPool = default;
        private readonly EcsPoolInject<ChargePointComponent> _chargePointPool = default;
        private readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationCooldownPool = default;
        private readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateDefenseContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var defenseContext = ref _defenseContextPool.Value.Get(unitEntity);
                bool anyActionAvailable = false;
                foreach (EcsPackedEntity packedEntity in defenseContext.defenseActionsList)
                {
                    if (IsActionAvailable(packedEntity))
                    {
                        anyActionAvailable = true;
                    }
                }
                defenseContext.anyActionAvailable = anyActionAvailable;
            }
        }
        private bool IsActionAvailable(EcsPackedEntity entity)
        {
            //TODOihor some evaluation here
            if (entity.Unpack(_world.Value, out var abilityEntity)) //TODOihor some processing here
            {

                if (_chargePointPool.Value.Has(abilityEntity))
                {
                    ref var chargepointComp = ref _chargePointPool.Value.Get(abilityEntity);
                    if (chargepointComp.CurrentChargeCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (!_recalculationCooldownPool.Value.Has(abilityEntity))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

    }
}