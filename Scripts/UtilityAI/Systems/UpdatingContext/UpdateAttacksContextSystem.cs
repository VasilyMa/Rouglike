using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// System updates info that relates to attacks usage and availability
    /// </summary>
    sealed class UpdateAttacksContextSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, AttacksContext, TargetsContext>> _aiAgentFilter = default;
        readonly EcsPoolInject<AttacksContext> _attacksContextPool = default;
        readonly EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<DesynchronizationComponent> _desyncPool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationCooldownPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateAttacksContextSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int aiAgentEntity in _aiAgentFilter.Value)
            {
                ref var attacksContext = ref _attacksContextPool.Value.Get(aiAgentEntity);
                ref var targetsContext = ref _targetsContextPool.Value.Get(aiAgentEntity);
                bool anyAttackAvailable = false;
                bool anyAttackUsable = false;
                foreach (EcsPackedEntity packedEntity in attacksContext.attackAbilitiesList)
                 {
                    if (IsAttackAvailable(packedEntity, ref targetsContext, ref attacksContext) && !_desyncPool.Value.Has(aiAgentEntity))
                    {
                        anyAttackAvailable = true;
                        if (IsAttackUsable(packedEntity, ref targetsContext))
                        {
                            if(!attacksContext.validAbilitiesList.Contains(packedEntity)) attacksContext.validAbilitiesList.Add(packedEntity);
                            anyAttackUsable = true;
                        }
                    }
                }
                attacksContext.anyActionAvailable = anyAttackAvailable;
                attacksContext.anyActionUsable = anyAttackUsable;
            }
        }
        private bool IsAttackAvailable(EcsPackedEntity entity, ref TargetsContext targetsContext, ref AttacksContext attackContext)
        {
            if (entity.Unpack(_world.Value, out var abilityEntity))
            {
                    if (!_recalculationCooldownPool.Value.Has(abilityEntity))
                    {
                        return true;
                    }
                    else
                    {
                        if (attackContext.validAbilitiesList.Contains(entity)) attackContext.validAbilitiesList.Remove(entity);
                        return false;
                    }
            }
            else
            {
                if (attackContext.validAbilitiesList.Contains(entity)) attackContext.validAbilitiesList.Remove(entity);
                return false;
            }
        }

        private bool IsAttackUsable(EcsPackedEntity entity, ref TargetsContext targetsContext)
        {
            if (entity.Unpack(_world.Value, out var abilityEntity))
            {
                ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                return true;
;
            }
            return false;
        }
    }
}