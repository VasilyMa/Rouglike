using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Client
{
    sealed class ExplosionOnHitSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly private EcsFilterInject<Inc<DamageEffect>> _filter = default;
        readonly private EcsPoolInject<DamageEffect> _pool = default;
        readonly private EcsPoolInject<HealthComponent> _healthPool = default;
        readonly private EcsPoolInject<MissileComponent> _missilePool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsPoolInject<AttackZoneComponent> _attackZonePool = default;
        readonly private EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<AbilityEffectsContainer> _abilityEffectsContainer = default;
        readonly private EcsPoolInject<RequestAttackZoneEvent> _requestAttackZone = default;
        readonly EcsPoolInject<CasterMissileComponent> _casterMissilePool;
        private const float PERCENTAGE = 100;
        public void Run(IEcsSystems systems)
        {
           /* foreach (var entity in _filter.Value)
            {
                ref var damageEffect = ref _pool.Value.Get(entity);
                if (damageEffect.SenderPackedEntity.Unpack(_world.Value, out int entitySender))
                {
                    if (_attackZonePool.Value.Has(entitySender))
                    {
                        ref var atackZoneComp = ref _attackZonePool.Value.Get(entitySender);
                        if (atackZoneComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                        {
                            if (_abilityEffectsContainer.Value.Has(ownerEntity))
                            {
                                ref var abilityEffectContainer = ref _abilityEffectsContainer.Value.Get(ownerEntity);
                                foreach (var abilityEffectConfig in abilityEffectContainer.AbilityEffectConfigs)
                                {
                                    if (abilityEffectConfig.effectType == EffectType.ExplosionOnHit)
                                    {
                                        if (atackZoneComp.AbilityEntity.Unpack(_world.Value, out int abilityEntity))
                                        {
                                            ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                                            if (abilityEffectConfig.HasInputFlag(abilityComp.Ability.SourceAbility.InputActionReference.action.name, abilityEffectConfig))
                                            {
                                                ref var transformComp = ref _transformPool.Value.Get(entity);
                                                if (abilityEffectConfig.Particle != null)
                                                {
                                                    GameState.Instance.CreatePool(abilityEffectConfig.Particle.gameObject, abilityEffectConfig.Particle.gameObject.name);
                                                    if (GameState.Instance.TryGetPool(abilityEffectConfig.Particle.gameObject, out var pool))
                                                    {
                                                        var particle = pool.GetFromPool();
                                                        particle.SetPositionAndRotation(transformComp.Transform.position + Vector3.up, Quaternion.identity);
                                                        particle.gameObject.SetActive(true);
                                                        particle.GetComponent<ParticleSystem>().Play();
                                                    }
                                                }
                                                var damageZone = GameObject.Instantiate(abilityEffectConfig.damageZone, transformComp.Transform.position, Quaternion.identity);
                                                damageZone.Damage = (int)abilityEffectConfig.value;
                                                damageZone.SenderEntity = ownerEntity;
                                                damageZone.TargetEntity = entity;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (_missilePool.Value.Has(entitySender))
                    {
                        ref var missileComp = ref _missilePool.Value.Get(entitySender);
                        if (!_casterMissilePool.Value.Has(entity)) return;
                        if (_abilityEffectsContainer.Value.Has(missileComp.EntityCaster))
                        {
                            ref var abilityComp = ref _abilityPool.Value.Get(missileComp.AbilityEntity);
                            ref var abilityEffectContainer = ref _abilityEffectsContainer.Value.Get(missileComp.EntityCaster);
                            foreach (var abilityEffectConfig in abilityEffectContainer.AbilityEffectConfigs)
                            {
                                if (abilityEffectConfig.effectType == EffectType.ExplosionOnHit)
                                {
                                    if (abilityEffectConfig.HasInputFlag(abilityComp.Ability.SourceAbility.InputActionReference.action.name, abilityEffectConfig))
                                    {
                                        ref var transformComp = ref _transformPool.Value.Get(entity);
                                        if (abilityEffectConfig.Particle != null)
                                        {
                                            GameState.Instance.CreatePool(abilityEffectConfig.Particle.gameObject, abilityEffectConfig.Particle.gameObject.name);
                                            if (GameState.Instance.TryGetPool(abilityEffectConfig.Particle.gameObject, out var pool))
                                            {
                                                var particle = pool.GetFromPool();
                                                particle.SetPositionAndRotation(transformComp.Transform.position + Vector3.up, Quaternion.identity);
                                                particle.gameObject.SetActive(true);
                                                particle.GetComponent<ParticleSystem>().Play();
                                            }
                                        }
                                        var damageZone = GameObject.Instantiate(abilityEffectConfig.damageZone, transformComp.Transform.position, Quaternion.identity);
                                        damageZone.Damage = (int)abilityEffectConfig.value;
                                        damageZone.SenderEntity = missileComp.EntityCaster;
                                        damageZone.TargetEntity = entity;
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }
    }
}