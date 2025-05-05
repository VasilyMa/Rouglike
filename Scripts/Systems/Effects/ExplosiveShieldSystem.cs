using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client
{
    sealed class ExplosiveShieldSystem : MainEcsSystem
    {
        readonly EcsWorldInject world = default;
        readonly EcsFilterInject<Inc<ShieldDestructionEvent, ShieldsContainer, ExplosiveAbilityComponent>> _filter;
        readonly EcsPoolInject<ExplosiveAbilityComponent> _explosiveAbilityPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new ExplosiveShieldSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var explosiveAbilityComp = ref _explosiveAbilityPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                var colliders = Physics.OverlapSphere(transformComp.Transform.position, explosiveAbilityComp.Radius, explosiveAbilityComp.LayerMask.value);

                Vector3 explosionPos = transformComp.Transform.position + new Vector3(0, 0.5f, 0);


                var hit = PoolModule.Instance.GetFromPool<SourceParticle>(explosiveAbilityComp.ExpolosiveEffect,true);

                hit.gameObject.SetActive(true);
                hit.AttachVisualEffectToEntity(explosionPos, Quaternion.identity, world.Value.PackEntity(entity));

                foreach (var collider in colliders)
                {
                    if (collider == null) continue;

                    if (collider.transform.TryGetComponent<UnitMB>(out var unit))
                    {
                        ref var takeDamageComp = ref world.Value.GetPool<TakeDamageComponent>().Add(world.Value.NewEntity());
                        takeDamageComp.Damage = explosiveAbilityComp.DamageValue;
                        takeDamageComp.KillerEntity = world.Value.PackEntity(entity);
                        takeDamageComp.TargetEntity = world.Value.PackEntity(unit._entity);
                    }
                }
            }
        }
    }
}