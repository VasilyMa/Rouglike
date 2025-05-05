using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    /// <summary>
    /// Reserve system for Self Context initializing. 
    /// Iterates through all entities with UnitBrain and SelfContext
    /// </summary>
    sealed class InitSelfContextSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, SelfContext, InitContextEvent>> _filter = default;
        readonly EcsPoolInject<SelfContext> _selfContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitSelfContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                
                
            }
        }
    }
}