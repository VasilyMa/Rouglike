using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddRotationPlayerSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<VIEWDIRECTIONInputEvent>> _inputFilter = default;
        readonly EcsPoolInject<VIEWDIRECTIONInputEvent> _viewDirectionPool = default;
        readonly EcsFilterInject<Inc<PlayerComponent>, Exc<LockRotationComponent, DeadComponent>> _filter = default;
        readonly EcsPoolInject<RotationComponent> _rotationPool = default;

        public override MainEcsSystem Clone()
        {
            return new AddRotationPlayerSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var inputEntity in _inputFilter.Value)
            {
                foreach (var entity in _filter.Value)
                {
                    ref var viewDirectionComp = ref _viewDirectionPool.Value.Get(inputEntity);

                    ref var rotationComp = ref _rotationPool.Value.Add(entity);
                    rotationComp.ViewDirection = viewDirectionComp.ViewDirection;
                }
            }
        }
    }
}