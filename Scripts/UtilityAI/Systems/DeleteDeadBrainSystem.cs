using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class DeleteDeadBrainSystem : MainEcsSystem 
    {
        readonly private EcsFilterInject<Inc<UnitBrain, DeadComponent>> _filter = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;

        public override MainEcsSystem Clone()
        {
            return new DeleteDeadBrainSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int deadUnitEntity in _filter.Value)
            {
                _brainPool.Value.Del(deadUnitEntity);
            }
        }
    }
}