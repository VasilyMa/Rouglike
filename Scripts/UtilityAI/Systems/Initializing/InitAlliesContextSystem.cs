using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
namespace Client {
    /// <summary>
    /// System for initializing Allies context.
    /// Catches InitAIEvent, iterates through all entities with UnitBrain and AlliesContext component
    /// </summary>
    sealed class InitAlliesContextSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<UnitBrain, AlliesContext, InitContextEvent>> _filter = default;
        readonly private EcsPoolInject<AlliesContext> _alliesContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitAlliesContextSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (int unitEntity in _filter.Value)
            {
                ref var alliesContext = ref _alliesContextPool.Value.Get(unitEntity);
                alliesContext.alliedEntitiesWithDistance = new Dictionary<EcsPackedEntity, float>();
            }
        }
    }
}