using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    /// <summary>
    /// System initializes ThreatsContext component. Catches InitAIEvent.
    /// Iterates through all Entities with UnitBrain and ThreatsContext components 
    /// </summary>
    sealed class InitThreatContextSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<UnitBrain, ThreatsContext, InitContextEvent>> _unitFilter = default;
        readonly private EcsPoolInject<ThreatsContext> _threatsContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitThreatContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _unitFilter.Value)
            {
                
            }
        }
    }
}