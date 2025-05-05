using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using UnityEngine;
using Statement;

namespace Client
{
    sealed class ShieldSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TakeDamageComponent, ShieldDamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<ShieldsContainer> _shieldContainerPool = default;
        readonly EcsPoolInject<ShieldDestructionEvent> _shieldDestructionPool;

        public override MainEcsSystem Clone()
        {
            return new ShieldSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int targetEntity))
                {
                    ref var shieldContainer = ref _shieldContainerPool.Value.Get(targetEntity);
                    foreach (var shield in shieldContainer.shieldComponents)
                    {
                        float serviceDamage = takeDamageComp.Damage - shield.DamageProtection;
                        shield.DamageProtection -= takeDamageComp.Damage;
                        takeDamageComp.Damage = Mathf.Clamp(serviceDamage, 0f, float.PositiveInfinity);

                        if(shield.DamageProtection <= 0)
                        {
                            if (!_shieldDestructionPool.Value.Has(entity)) _shieldDestructionPool.Value.Add(entity).shields = new();
                            ref var shieldDestruction = ref _shieldDestructionPool.Value.Get(entity);
                            shieldDestruction.shields.Add(shield);
                        }
                    }
                }
            }
        }
    }
}