using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class WanderingRequestProcessingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, WanderingRequest, TargetsContext>,Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly private EcsPoolInject<WanderingTag> _wanderingPool = default;
        readonly private EcsPoolInject<StartNavigationRequest> _startNavigationRequestPool = default;
        readonly private EcsPoolInject<TargetsContext> _targetsContextPool = default;
        readonly private EcsWorldInject _world = default;
        readonly EcsPoolInject<MoveAnimationState> _moveAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new WanderingRequestProcessingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var startNavigationRequest = ref _startNavigationRequestPool.Value.Add(_world.Value.NewEntity());
                startNavigationRequest.packedEntity = _world.Value.PackEntity(unitEntity);
                ref var targetsContext = ref _targetsContextPool.Value.Get(unitEntity);
                if (!_wanderingPool.Value.Has(unitEntity))
                {
                    ref var wandering = ref _wanderingPool.Value.Add(unitEntity);
                }
                _moveAnimationPool.Value.Add(unitEntity);
            }
        }
    }
}