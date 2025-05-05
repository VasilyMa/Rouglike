using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class EvaluateInActionRotateSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitBrain, InActionComponent,TargetsContext>> _filter = default;
        readonly EcsPoolInject<UnitBrain> _brainPool = default;
        readonly EcsPoolInject<TargetsContext> _targetContextPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new EvaluateInActionRotateSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var unitEntity in _filter.Value)
            {
                ref var targetContext = ref _targetContextPool.Value.Get(unitEntity);
                ref var unitBrain = ref _brainPool.Value.Get(unitEntity);
                if (targetContext.closestEnemyEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var targetTransform = ref _transformPool.Value.Get(targetEntity);
                    unitBrain.priorityPointToLook = targetTransform.Transform.position;
                }
            }
        }
    }
}