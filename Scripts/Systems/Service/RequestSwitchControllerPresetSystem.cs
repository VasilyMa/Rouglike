using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class RequestSwitchControllerPresetSystem : MainEcsSystem 
    {
        private readonly EcsFilterInject<Inc<RequestSwithControllerEvent>> _filter = default;
        private readonly EcsFilterInject<Inc<InputComponent>> _inputFilter = default;
        private readonly EcsPoolInject<InputComponent> _inputPool = default;
        private readonly EcsPoolInject<RequestSwithControllerEvent> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new RequestSwitchControllerPresetSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var requestComp = ref _pool.Value.Get(entity);
                foreach (var inputEntity in _inputFilter.Value)
                {
                    ref var inputComp = ref _inputPool.Value.Get(inputEntity);
                    inputComp.SetInputActionPreset(requestComp.InputActionPreset);
                    _pool.Value.Del(entity);
                }
            }
        }
    }
}