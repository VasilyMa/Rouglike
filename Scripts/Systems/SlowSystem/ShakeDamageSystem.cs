using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ShakeDamageSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world;
        readonly EcsFilterInject<Inc<TakeDamageComponent, ShakeCameraAllowedComponent>> _filterDamage;
        readonly EcsPoolInject<ShakeCameraEvent> _shakePool;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool;

        public override MainEcsSystem Clone()
        {
            return new ShakeDamageSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filterDamage.Value)
            {
                ref var damageComp = ref _takeDamagePool.Value.Get(entity);
                var config = ConfigModule.GetConfig<ViewConfig>().SlowVisualConfig;
                if (damageComp.Damage >= config.ShakeStrongDanage)
                {
                    _shakePool.Value.Add(_world.Value.NewEntity());
                }
            }
        }
    }
}