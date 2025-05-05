using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpdateFromPlaceContextSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, FromPlaceContext, TargetsContext, AttacksContext>> _aiAgentFilter = default;
        readonly EcsPoolInject<FromPlaceContext> _fromPlaceContextPool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationCooldownPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateFromPlaceContextSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int aiAgentEntity in _aiAgentFilter.Value)
            {
                ref var fromPlaceContext = ref _fromPlaceContextPool.Value.Get(aiAgentEntity);
                bool anyAttackUsable = false;
                if (fromPlaceContext.fromPlaceTransforms is null)  return;
                if (fromPlaceContext.fromPlaceTransforms.Count < 1) return;
                foreach (EcsPackedEntity packedEntity in fromPlaceContext.fromPlaceAbilitiesList)
                {
                    if (IsFromPlaceAvailable(packedEntity, ref fromPlaceContext))
                    {
                        if (!fromPlaceContext.validAbilitiesList.Contains(packedEntity)) fromPlaceContext.validAbilitiesList.Add(packedEntity);
                        anyAttackUsable = true;
                    }
                }
                fromPlaceContext.AnyActionUsable = anyAttackUsable;
            }
        }
        private bool IsFromPlaceAvailable(EcsPackedEntity entity, ref FromPlaceContext fromPLaceContext)
        {
            if (entity.Unpack(_world.Value, out var abilityEntity))
            {
                if (!_recalculationCooldownPool.Value.Has(abilityEntity))
                {
                    return true;
                }
                else
                {
                    if (fromPLaceContext.validAbilitiesList.Contains(entity)) fromPLaceContext.validAbilitiesList.Remove(entity);
                    return false;
                }
            }
            else
            {
                if (fromPLaceContext.validAbilitiesList.Contains(entity)) fromPLaceContext.validAbilitiesList.Remove(entity);
                return false;
            }
        }
    }
}