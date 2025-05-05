using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    /// <summary>
    /// System adds new entry in AlliesContext entities list every time an AI agent being initialized.
    /// System catches InitAIEvent and iterates through all entities with UnitBrain and AlliesContext.
    /// </summary>
    sealed class FillAlliesListSystem : MainEcsSystem
    {
        readonly private EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitAIEvent>> _filter = default;
        //TODOihor probably need to specify which AI agents should be considered as supportable entities
        readonly EcsFilterInject<Inc<UnitBrain, AlliesContext>> _unitsFilter = default;
        readonly private EcsPoolInject<AlliesContext> _alliesContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new FillAlliesListSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _unitsFilter.Value)
            {
                ref var alliesContext = ref _alliesContextPool.Value.Get(unitEntity);
                foreach (int eventEntity in _filter.Value)
                {
                    alliesContext.alliedEntitiesWithDistance.Add(_world.Value.PackEntity(eventEntity), float.MaxValue);
                }
            }
        }
    }
}