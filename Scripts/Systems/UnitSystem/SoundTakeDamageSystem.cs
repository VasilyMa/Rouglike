using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SoundTakeDamageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, SoundDamageAllowedComponent>, Exc<ConditionTakeDamageComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<SoundUnitComponent> _soundUnitPool = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, SoundDamageAllowedComponent, ConditionTakeDamageComponent>> _conditionDamageFilter = default;
        public override MainEcsSystem Clone()
        {
            return new SoundTakeDamageSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var soundComp = ref _soundUnitPool.Value.Get(targetEntity);
                    soundComp.SoundUnitMB.PlayDamageSound();
                }
            }

            foreach (int entity in _conditionDamageFilter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if (takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    //TODO play condition sounds
                }
            }
        }
    }
}