using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;


namespace Client {
    sealed class LightningMissileSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<LightningMissileEvent>> _filter = default;
        readonly EcsPoolInject<LightningMissileEvent> _lightningPool = default;

        public override MainEcsSystem Clone()
        {
            return new LightningMissileSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var lightningMisileComp = ref _lightningPool.Value.Get(entity);

                lightningMisileComp.TimeLife -= Time.deltaTime;

                if (lightningMisileComp.TimeLife <= 0)
                {
                    lightningMisileComp.Missile.FinishMissile();
                    _lightningPool.Value.Del(entity);
                }
            }
        }
    }
}