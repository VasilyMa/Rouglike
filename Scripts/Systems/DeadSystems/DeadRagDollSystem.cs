using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DeadRagDollSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PhysicsUnitComponent, MomentDeadEvent>> _filter;
        readonly EcsPoolInject<PhysicsUnitComponent> _phisicsUnitPool = default;

        public override MainEcsSystem Clone()
        {
            return new DeadRagDollSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var phisicsUnitComp = ref _phisicsUnitPool.Value.Get(entity);
                phisicsUnitComp.PhysicsUnitMB.KillUnit();
            }
        }
    }
}