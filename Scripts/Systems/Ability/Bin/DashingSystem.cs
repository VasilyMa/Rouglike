using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class DashingSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<Dashing>> _dashingFilter = default;
        readonly private EcsPoolInject<Dashing> _dashingPool = default;

        public override MainEcsSystem Clone()
        {
            return new DashingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _dashingFilter.Value)
            {
                ref var dashing = ref _dashingPool.Value.Get(entity);
                dashing.duration -= Time.deltaTime;
                if (dashing.duration > 0) continue;
                _dashingPool.Value.Del(entity);
            }
        }
    }
}