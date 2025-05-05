using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class UpdateTargetsContextSystem : MainEcsSystem 
    {
        readonly private EcsFilterInject<Inc<UnitBrain, TargetsContext, TransformComponent>> _aiAgentFilter = default;
        readonly private EcsFilterInject<Inc<PlayerComponent, TransformComponent>> _hostileFilter = default; //TODOihor implement filter for all hostile entitites
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateTargetsContextSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (int aiAgentEntity in _aiAgentFilter.Value)
            {
                ref var targetsContext = ref _targetsContextPool.Value.Get(aiAgentEntity);
                ref var transformComp = ref _transformPool.Value.Get(aiAgentEntity);
                targetsContext.closestEnemyDistance = float.MaxValue;
                foreach (int hostileEntity in _hostileFilter.Value)
                {
                    ref var hostileTransformComp = ref _transformPool.Value.Get(hostileEntity);
                    float distance = (hostileTransformComp.Transform.position - transformComp.Transform.position).magnitude;
                    if (distance < targetsContext.closestEnemyDistance)
                    {
                        targetsContext.closestEnemyDistance = distance;
                        targetsContext.closestEnemyEntity = _world.Value.PackEntity(hostileEntity);
                    }
                }
            }
        }
    }
}