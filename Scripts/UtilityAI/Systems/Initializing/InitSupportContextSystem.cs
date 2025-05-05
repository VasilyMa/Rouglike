using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
namespace Client {
    sealed class InitSupportContextSystem : MainEcsSystem {
        /// <summary>
        /// Reserve system for Support Context initializing. 
        /// Iterates through all entities with UnitBrain and SupportContext
        /// </summary>
        readonly private EcsFilterInject<Inc<UnitBrain, SupportContext, InitContextEvent>> _filter = default;
        readonly private EcsPoolInject<SupportContext> _supportContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitSupportContextSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int unitEntity in _filter.Value)
            {
                ref var supportContext = ref _supportContextPool.Value.Get(unitEntity);
                supportContext.supportAbilitiesList = new List<EcsPackedEntity>();
            }
        }
    }
}