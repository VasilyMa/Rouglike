using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;

namespace Client {
    sealed class SlowSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<SlowComponent>> _filter;
        readonly EcsPoolInject<SlowComponent> _slowPool;
        private float _defaultFixedDT = 0.02f;
        private float _defaultVFXDT = 0.01666667f;

        public override MainEcsSystem Clone()
        {
            return new SlowSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var slowComp = ref _slowPool.Value.Get(entity);
                CinemachineImpulseManager.Instance.Clear();
                slowComp.Duration -= Time.deltaTime;
                if(slowComp.Active)
                {
                    slowComp.Active = false;
                    Time.timeScale = slowComp.Value;
                    Time.fixedDeltaTime = _defaultFixedDT * Time.timeScale;
                    VFXManager.fixedTimeStep = _defaultVFXDT * Time.timeScale;
                }
                if(slowComp.Duration <= 0)
                {
                    if(_filter.Value.GetEntitiesCount() == 1)
                    {
                        Time.timeScale = 1;
                        Time.fixedDeltaTime = _defaultFixedDT;
                        VFXManager.fixedTimeStep = _defaultVFXDT;
                    }
                    _slowPool.Value.Del(entity);
                }
            }
        }
    }
}