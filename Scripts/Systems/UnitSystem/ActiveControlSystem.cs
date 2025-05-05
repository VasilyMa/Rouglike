using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ActiveControlSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ActiveControlEvent>, Exc<HardControlComponent>> _filter = default;
        readonly EcsPoolInject<LockMoveComponent> _lockMovePool = default;
        readonly EcsPoolInject<LockRotationComponent> _lockRotationPool = default;

        public override MainEcsSystem Clone()
        {
            return new ActiveControlSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                _lockMovePool.Value.Del(entity);
                _lockRotationPool.Value.Del(entity);
            }
        }
    }
}