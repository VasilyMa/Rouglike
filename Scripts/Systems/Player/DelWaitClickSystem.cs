using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DelWaitClickSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DelWaitClick, WaitClick, PlayerComponent>> _filter = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<WaitClick> _waitClickPool = default;

        public override MainEcsSystem Clone()
        {
            return new DelWaitClickSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var unitMBComp = ref _unitMBPool.Value.Get(entity);
                unitMBComp.AbilityUnitMB.CurrentAbility = string.Empty;

                _waitClickPool.Value.Del(entity);
            }
        }
    }
}