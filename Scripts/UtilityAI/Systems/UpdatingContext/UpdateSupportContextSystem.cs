using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    /// <summary>
    /// System updates support context that holds support action status and other info.
    /// System constantly iterates through all entities with UnitBrain, SupportContext and AlliesContext.
    /// </summary>
    sealed class UpdateSupportContextSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, SupportContext, AlliesContext>> _filter = default;
        readonly private EcsPoolInject<SupportContext> _supportContextPool = default;
        readonly private EcsPoolInject<AlliesContext> _alliesContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateSupportContextSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var supportContext = ref _supportContextPool.Value.Get(unitEntity);
                bool anyActionAvailable = false;
                bool anyActionUsable = false;
                foreach (EcsPackedEntity packedEntity in supportContext.supportAbilitiesList)
                {
                    if (IsActionAvailable(packedEntity))
                    {
                        anyActionAvailable = true;
                        if (IsActionUsable(packedEntity))
                        {
                            anyActionUsable |= true;
                        }
                    }
                }
                supportContext.anyActionAvailable = anyActionAvailable;
                supportContext.anyActionUsable = anyActionUsable;
            }
        }

        private bool IsActionAvailable(EcsPackedEntity packedEntity)
        {
            //TODOihor some processing
            return true;
        }

        private bool IsActionUsable(EcsPackedEntity packedEntity)
        {
            //TODOihor some processing
            return true;
        }
    }
}