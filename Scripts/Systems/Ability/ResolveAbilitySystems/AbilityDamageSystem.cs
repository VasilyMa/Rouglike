using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class AbilityDamageSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly private EcsFilterInject<Inc<DamageEffect>, Exc<TakeDamageComponent, ConditionTakeDamageComponent>> _filter = default;
        readonly private EcsPoolInject<DamageEffect> _pool = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<ConditionTakeDamageComponent> _conditionTakeDamagePool = default;

        public override MainEcsSystem Clone()
        {
            return new AbilityDamageSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var damageEffect = ref _pool.Value.Get(entity);
                ref var takeDamageComp = ref _takeDamagePool.Value.Add(entity);
                takeDamageComp.KillerEntity = damageEffect.SenderPackedEntity;
                takeDamageComp.Damage = damageEffect.DamageFinalValue;
                takeDamageComp.TargetEntity = damageEffect.TargetPackedEntity;
                
                if(damageEffect.IsConditionDamage) _conditionTakeDamagePool.Value.Add(entity);

            }
        }
    }
}