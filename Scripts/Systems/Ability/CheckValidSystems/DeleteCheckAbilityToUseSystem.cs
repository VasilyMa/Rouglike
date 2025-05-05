using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DeleteCheckAbilityToUseSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<CheckAbilityToUse> _checkPool = default;
        readonly EcsPoolInject<IsPressedAbilityComponent> _isPressedPool = default;

        public override MainEcsSystem Clone()
        {
            return new DeleteCheckAbilityToUseSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _checkPool.Value.Del(entity);
                _isPressedPool.Value.Del(entity);
            }
        }
    }
}