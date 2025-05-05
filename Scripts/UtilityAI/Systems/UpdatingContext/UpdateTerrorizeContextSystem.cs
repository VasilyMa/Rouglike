using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;

namespace Client {
    sealed class UpdateTerrorizeContextSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<UnitBrain, TerrorizeContext, TargetsContext, AttacksContext>> _aiAgentFilter = default;
        readonly EcsPoolInject<TerrorizeContext> _terrorizeContextPool = default;
        readonly EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationCooldownPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateTerrorizeContextSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (int aiAgentEntity in _aiAgentFilter.Value)
            {
                ref var terrorizeContext = ref _terrorizeContextPool.Value.Get(aiAgentEntity);
                ref var attackContext = ref _attacksContextPool.Value.Get(aiAgentEntity);
                bool anyAttackUsable = false;
                foreach (EcsPackedEntity packedEntity in terrorizeContext.terrorizeAbilitiesList)
                {
                    if (IsTerrorizeAvailable(packedEntity,ref terrorizeContext) && !attackContext.anyActionUsable)
                    {
                            if (!terrorizeContext.validAbilitiesList.Contains(packedEntity)) terrorizeContext.validAbilitiesList.Add(packedEntity);
                            anyAttackUsable = true;
                    }
                }
                terrorizeContext.anyActionUsable = anyAttackUsable;
            }
        }
        private bool IsTerrorizeAvailable(EcsPackedEntity entity, ref TerrorizeContext terrorizeContext)
        {
            if (entity.Unpack(_world.Value, out var abilityEntity))
            {
                if (!_recalculationCooldownPool.Value.Has(abilityEntity))
                {
                    return true;
                }
                else
                {
                    if (terrorizeContext.validAbilitiesList.Contains(entity)) terrorizeContext.validAbilitiesList.Remove(entity);
                    return false;
                }
            }
            else
            {
                if (terrorizeContext.validAbilitiesList.Contains(entity)) terrorizeContext.validAbilitiesList.Remove(entity);
                return false;
            }
        }
    }
}