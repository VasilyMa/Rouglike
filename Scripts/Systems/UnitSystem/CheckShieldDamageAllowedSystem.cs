using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class CheckShieldDamageAllowedSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TakeDamageComponent, ShieldDamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<Invulnerable> _invulnerablePool = default;
        readonly EcsPoolInject<ShieldDamageAllowedComponent> _allowedPool = default;
        readonly EcsPoolInject<ShieldsContainer> _shieldsContainerPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new CheckShieldDamageAllowedSystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int targetEntity))
                {
                    if(_invulnerablePool.Value.Has(targetEntity)) _allowedPool.Value.Del(entity);
                    if(!_shieldsContainerPool.Value.Has(targetEntity)) _allowedPool.Value.Del(entity);
                }
            }
        }
    }
}