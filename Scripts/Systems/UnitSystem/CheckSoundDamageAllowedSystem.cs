using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckSoundDamageAllowedSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, SoundDamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<SoundUnitComponent> _soundUnitPool = default;
        readonly EcsPoolInject<SoundDamageAllowedComponent> _soundAllowedPool = default;
        readonly EcsPoolInject<GlobalDamageCDComponent> _damageImmunityInterval;
        public override MainEcsSystem Clone()
        {
            return new CheckSoundDamageAllowedSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(!_soundUnitPool.Value.Has(targetEntity)) _soundAllowedPool.Value.Del(entity);
                    if (_damageImmunityInterval.Value.Has(targetEntity)) _soundAllowedPool.Value.Del(entity);
                }
            }
        }
    }
}