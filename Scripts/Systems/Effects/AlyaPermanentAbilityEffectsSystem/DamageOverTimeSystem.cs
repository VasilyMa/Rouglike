using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamageOverTimeSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly private EcsFilterInject<Inc<DamageEffect, DeadComponent>> _filter = default;
        readonly private EcsFilterInject<Inc<DamageOverTimeEffect>, Exc<DeadComponent>> _dotFilter = default;
        readonly private EcsPoolInject<DamageEffect> _pool = default;
        readonly private EcsPoolInject<HealthComponent> _healthPool = default;
        readonly private EcsPoolInject<MissileComponent> _missilePool = default;
        readonly private EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly private EcsPoolInject<AttackZoneComponent> _attackZonePool = default;
        readonly private EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly private EcsPoolInject<DamageOverTimeEffect> _DoTPool = default;
        readonly private EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<AbilityEffectsContainer> _abilityEffectsContainer = default;
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
                                    if (abilityEffectConfig.effectType == EffectType.DamageOverTime)
                                    {
                                        ref var transformComp = ref _transformPool.Value.Get(entity);
                                        if (!_DoTPool.Value.Has(entity))
                                        {
                                            ref var dotComp = ref _DoTPool.Value.Add(entity);
                                            dotComp.DamageOverTime = (int)abilityEffectConfig.value;
                                            dotComp.Duration = abilityEffectConfig.duration;
                                            dotComp.SenderEntity = _world.Value.PackEntity(entitySender);

                                            GameState.Instance.CreatePool(abilityEffectConfig.BleendingGO, abilityEffectConfig.BleendingGO.name);
                                            if (GameState.Instance.TryGetPool(abilityEffectConfig.BleendingGO, out var pool))
                                            {
                                                var particle = pool.GetFromPool();
                                                particle.position = transformComp.Transform.position;
                                                particle.parent = transformComp.Transform;
                                                particle.gameObject.SetActive(true);
                                                dotComp.Paricle = particle.gameObject;
                                            }

                                        }
                                        else
                                        {
                                            ref var dotComp = ref _DoTPool.Value.Get(entity);
                                            dotComp.Duration = abilityEffectConfig.duration;
                                            dotComp.SenderEntity = _world.Value.PackEntity(entitySender);
                                            dotComp.Paricle.SetActive(false);

                                            GameState.Instance.CreatePool(abilityEffectConfig.BleendingGO, abilityEffectConfig.BleendingGO.name);
                                            if (GameState.Instance.TryGetPool(abilityEffectConfig.BleendingGO, out var pool))
                                            {
                                                var particle = pool.GetFromPool();
                                                particle.position = transformComp.Transform.position;
                                                particle.parent = transformComp.Transform;
                                                particle.gameObject.SetActive(true);
                                                dotComp.Paricle = particle.gameObject;
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
                            ref var abilityEffectContainer = ref _abilityEffectsContainer.Value.Get(missileComp.EntityCaster);
                            foreach (var abilityEffectConfig in abilityEffectContainer.AbilityEffectConfigs)
                            {
                                if (abilityEffectConfig.effectType == EffectType.DamageOverTime)
                                {
                                    ref var transformComp = ref _transformPool.Value.Get(entity);
                                    if (!_DoTPool.Value.Has(entity))
                                    {
                                        ref var dotComp = ref _DoTPool.Value.Add(entity);
                                        dotComp.DamageOverTime = (int)abilityEffectConfig.value;
                                        dotComp.Duration = abilityEffectConfig.duration;
                                        dotComp.SenderEntity = _world.Value.PackEntity(entitySender);

                                        GameState.Instance.CreatePool(abilityEffectConfig.BleendingGO, abilityEffectConfig.BleendingGO.name);
                                        if (GameState.Instance.TryGetPool(abilityEffectConfig.BleendingGO, out var pool))
                                        {
                                            var particle = pool.GetFromPool();
                                            particle.position = transformComp.Transform.position;
                                            particle.parent = transformComp.Transform;
                                            particle.gameObject.SetActive(true);
                                            dotComp.Paricle = particle.gameObject;
                                        }
                                    }
                                    else
                                    {
                                        ref var dotComp = ref _DoTPool.Value.Get(entity);
                                        dotComp.Duration = abilityEffectConfig.duration;
                                        dotComp.SenderEntity = _world.Value.PackEntity(entitySender);
                                        GameState.Instance.CreatePool(abilityEffectConfig.BleendingGO, abilityEffectConfig.BleendingGO.name);
                                        if (GameState.Instance.TryGetPool(abilityEffectConfig.BleendingGO, out var pool))
                                        {
                                            var particle = pool.GetFromPool();
                                            particle.position = transformComp.Transform.position;
                                            particle.parent = transformComp.Transform;
                                            particle.gameObject.SetActive(true);
                                            dotComp.Paricle = particle.gameObject;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var entity in _dotFilter.Value)
            {
                ref var dotComp = ref _DoTPool.Value.Get(entity);
                if (dotComp.timer >= 1)
                {
                    dotComp.timer = 0;
                    ref var takeDamageComp = ref _takeDamagePool.Value.Add(_world.Value.NewEntity());
                    takeDamageComp.Damage = dotComp.DamageOverTime;
                    takeDamageComp.KillerEntity = dotComp.SenderEntity;
                    takeDamageComp.TargetEntity = _world.Value.PackEntity(entity);
                }
                else
                {
                    dotComp.timer += Time.deltaTime;
                }

                if (dotComp.durationTimer >= dotComp.Duration)
                {
                    dotComp.Paricle.SetActive(false);
                    _DoTPool.Value.Del(entity);
                }
                else
                    dotComp.durationTimer += Time.deltaTime;
            }*/

        }
    }
}