using Cinemachine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class ShakeCameraSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world;
        readonly EcsFilterInject<Inc<ShakeCameraEvent>> _filter;
        readonly EcsPoolInject<ShakeCameraEvent> _shakePool;
        readonly EcsFilterInject<Inc<TakeDamageComponent>> _filterDamage;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool;

        public override MainEcsSystem Clone()
        {
            return new ShakeCameraSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                _shakePool.Value.Del(entity);
            }
        }
    }
}