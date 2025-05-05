using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class AfterDamagePerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterDamagePerk>, Exc<UnusedPerk>> _filter = default;
        readonly EcsPoolInject<AfterDamagePerk> _afterDamagePool = default;
        readonly EcsPoolInject<UnusedPerk> _unusedPool = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent>> _takeDamageFilter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;

        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterDamagePerkSystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var perkEntity in _filter.Value)
            {
                ref var afterDamageComp = ref _afterDamagePool.Value.Get(perkEntity);
                foreach(var takeDamageEntity in _takeDamageFilter.Value)
                {
                    ref var takeDamageComp = ref _takeDamagePool.Value.Get(takeDamageEntity);
                    if(takeDamageComp.TargetEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int targetEntity))
                    {
                        if(targetEntity == BattleState.Instance.GetEntity("PlayerEntity")) 
                            afterDamageComp.CurrentDamage += takeDamageComp.Damage;
                    }
                }

                if(afterDamageComp.CurrentDamage >= afterDamageComp.DamageTargetValue)
                {
                    afterDamageComp.CurrentDamage = 0f;
                }
                else
                {
                    _unusedPool.Value.Add(perkEntity);
                }
            }
        }
    }
}