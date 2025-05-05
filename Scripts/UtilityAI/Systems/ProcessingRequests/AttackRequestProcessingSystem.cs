using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AttackRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<AttackRequest, UnitBrain>> _filter = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavigationRequestPool = default;
        readonly private EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContext = default;
        readonly private EcsPoolInject<AttackTag> _attackTagPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new AttackRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // request for navigation agent stopping
                ref var stopRequest = ref _stopNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                stopRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                // start attack sequence
                ref var brainComp = ref _unitBrainPool.Value.Get(unitEntity);
                ref var targetsContext = ref _targetsContext.Value.Get(unitEntity);
                // brainComp.bestAttackAvailable => best attackAbility entity
                if (targetsContext.closestEnemyEntity.Unpack(_world.Value, out int enemyEntity))
                {
                    if (_transformPool.Value.Has(enemyEntity))
                    {
                        if (!_attackTagPool.Value.Has(unitEntity))
                        {
                            _attackTagPool.Value.Add(unitEntity); //initial left to right direction randomize
                            //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Attack, unitEntity);
                        }
                        /*ref var keepingAtRange = ref _keepingAtRangePool.Value.Get(unitEntity);
                        keepingAtRange.transformToKeepAtRange = _transformPool.Value.Get(enemyEntity).Transform;
                        keepingAtRange.distanceToKeep = 5f; //TODOihor get this distance somewhere else*/
                    }
                }
                // request rotation if needed ??? 
            }
        }
    }
}