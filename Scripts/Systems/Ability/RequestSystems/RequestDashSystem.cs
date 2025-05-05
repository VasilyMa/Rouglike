using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class RequestDashSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<RequestDash>, Exc<Dashing>> _requestDashFilter = default;
        readonly private EcsPoolInject<RequestDash> _requestDashPool = default;
        readonly private EcsPoolInject<Dashing> _dashingPool = default;
        readonly private EcsPoolInject<TransformComponent> _transfPool = default;
        readonly private EcsFilterInject<Inc<WASDInputEvent>> _wasdFilter = default;
        readonly private EcsPoolInject<WASDInputEvent> _wasdPool = default;
        private EcsPoolInject<HardControlComponent> _hardControlPool;
        private EcsPoolInject<ApprovedDashAfterHitComponent> _approvedDashPool;
        
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new RequestDashSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _requestDashFilter.Value)
            {
                 if (_hardControlPool.Value.Has(entity) && !_approvedDashPool.Value.Has(entity)) continue;
                foreach (var input in _wasdFilter.Value)
                {
                    ref var wasd = ref _wasdPool.Value.Get(input);
                    ref var trans = ref _transfPool.Value.Get(entity);
                    //trans.Transform.LookAt(trans.Transform.position + wasd.WasdDirection);
                }
                _dashingPool.Value.Add(entity).duration = _requestDashPool.Value.Get(entity).Duration;
            }
        }
    }
}