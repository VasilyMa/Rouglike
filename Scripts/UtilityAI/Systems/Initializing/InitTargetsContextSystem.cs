using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    /// <summary>
    /// Reserve system for Threat Context initializing. 
    /// Iterates through all entities with UnitBrain and ThreatContext
    /// </summary>
    sealed class InitTargetsContextSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<UnitBrain, TargetsContext, InitContextEvent>> _filter = default;
        readonly EcsPoolInject<TargetsContext> _attackContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitTargetsContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                
            }
        }
    }
}