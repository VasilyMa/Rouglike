using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class KeepAtRangeRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, KeepAtRangeRequest, TargetsContext>,Exc<InActionComponent, HardControlComponent, MoveAnimationState>> _filter = default;
        readonly private EcsPoolInject<KeepingAtRangeTag> _keepingAtRangePool = default;
        readonly private EcsPoolInject<StartNavigationRequest> _startNavigationRequestPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsWorldInject _world = default;
        readonly EcsPoolInject<MoveAnimationState> _moveAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new KeepAtRangeRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                // switch to kkeping at range state that is similar to current Circling state 
                ref var startNavigationRequest = ref _startNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                startNavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                ref var targetsContext = ref _targetsContextPool.Value.Get(unitEntity);
                // => rewrite Circling related systems
                if (targetsContext.closestEnemyEntity.Unpack(_world.Value, out int enemyEntity))
                {
                    if (_transformPool.Value.Has(enemyEntity))
                    {
                        if (!_keepingAtRangePool.Value.Has(unitEntity))
                        {
                            _keepingAtRangePool.Value.Add(unitEntity).directionModifier = Random.Range(0, 2) * 2 - 1; //initial left to right direction randomize
                        }

                        _moveAnimationPool.Value.Add(unitEntity);
                        //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Move, unitEntity);

                        ref var keepingAtRange = ref _keepingAtRangePool.Value.Get(unitEntity);
                        keepingAtRange.transformToKeepAtRange = _transformPool.Value.Get(enemyEntity).Transform;
                        keepingAtRange.distanceToKeep = 5f; //TODOihor get this distance somewhere else
                    }
                }
            }
        }
    }
}