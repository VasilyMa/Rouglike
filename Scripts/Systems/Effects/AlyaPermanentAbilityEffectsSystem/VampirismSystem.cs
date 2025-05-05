using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class VampirismSystem : IEcsRunSystem
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
        private const float PERCENTAGE = 100;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
               /* ref var healthplayer = ref _healthPool.Value.Get(GameState.Instance.PlayerEntity);
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
                                    if (abilityEffectConfig.effectType == EffectType.Vampirism)
                                    {
                                        if (atackZoneComp.AbilityEntity.Unpack(_world.Value, out int abilityEntity))
                                        {
                                            ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                                            if (abilityEffectConfig.HasInputFlag(abilityComp.Ability.SourceAbility.InputActionReference.action.name, abilityEffectConfig))
                                            {
                                                ref var healthCompSender = ref _healthPool.Value.Get(ownerEntity);
                                                var healtAdded = healthCompSender.TakeDamageReturnCurrent(-damageEffect.DamageFinalValue * (abilityEffectConfig.value / PERCENTAGE));
                                                if (abilityEffectConfig.Particle)
                                                {
                                                    ref var transformComp = ref _transformPool.Value.Get(ownerEntity);
                                                    GameState.Instance.CreatePool(abilityEffectConfig.Particle.gameObject, abilityEffectConfig.Particle.gameObject.name);
                                                    if (GameState.Instance.TryGetPool(abilityEffectConfig.Particle.gameObject, out var pool))
                                                    {
                                                        var particle = pool.GetFromPool();
                                                        particle.SetPositionAndRotation(transformComp.Transform.position + Vector3.up, Quaternion.identity);
                                                        particle.gameObject.SetActive(true);
                                                        particle.GetComponent<ParticleSystem>().Play();
                                                    }
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
                            if (_abilityEffectsContainer.Value.Has(missileComp.EntityCaster))
                            {
                                ref var abilityEffectContainer = ref _abilityEffectsContainer.Value.Get(missileComp.EntityCaster);
                                foreach (var abilityEffectConfig in abilityEffectContainer.AbilityEffectConfigs)
                                {
                                    if (abilityEffectConfig.effectType == EffectType.Vampirism)
                                    {
                                        if (atackZoneComp.AbilityEntity.Unpack(_world.Value, out int abilityEntity))
                                        {
                                            ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);
                                            if (abilityEffectConfig.HasInputFlag(abilityComp.Ability.SourceAbility.InputActionReference.action.name, abilityEffectConfig))
                                            {
                                                ref var healthCompSender = ref _healthPool.Value.Get(ownerEntity);
                                                var healtAdded = healthCompSender.TakeDamageReturnCurrent(-damageEffect.DamageFinalValue * (abilityEffectConfig.value / PERCENTAGE));
                                                if (abilityEffectConfig.Particle)
                                                {
                                                    ref var transformComp = ref _transformPool.Value.Get(ownerEntity);
                                                    GameState.Instance.CreatePool(abilityEffectConfig.Particle.gameObject, abilityEffectConfig.Particle.gameObject.name);
                                                    if (GameState.Instance.TryGetPool(abilityEffectConfig.Particle.gameObject, out var pool))
                                                    {
                                                        var particle = pool.GetFromPool();
                                                        particle.SetPositionAndRotation(transformComp.Transform.position + Vector3.up, Quaternion.identity);
                                                        particle.gameObject.SetActive(true);
                                                        particle.GetComponent<ParticleSystem>().Play();
                                                    }
                                                }
                                            }
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
}