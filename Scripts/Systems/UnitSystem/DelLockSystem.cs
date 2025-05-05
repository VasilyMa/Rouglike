using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DelLockInActionSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DelLockInActionEvent>> _filter = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;
        readonly EcsPoolInject<InActionAnimationState> _inActionAnimationPool = default;

        public override MainEcsSystem Clone()
        {
            return new DelLockInActionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _inActionPool.Value.Del(entity);

                if (!_inActionAnimationPool.Value.Has(entity))
                {
                    _inActionAnimationPool.Value.Add(entity);
                }
            }
        }
    }
}