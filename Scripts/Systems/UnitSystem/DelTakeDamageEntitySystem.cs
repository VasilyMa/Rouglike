using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class DelTakeDamageEntitySystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<TakeDamageComponent>, Exc<ClearAllAllowedComponents>> _filter = default;
        readonly EcsPoolInject<ClearAllAllowedComponents> _clearPool = default;
        public override MainEcsSystem Clone()
        {
            return new DelTakeDamageEntitySystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _clearPool.Value.Add(entity);
            }
        }
    }
}