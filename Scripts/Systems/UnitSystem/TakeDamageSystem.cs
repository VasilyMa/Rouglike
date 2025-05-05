
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

using UnityEngine;

namespace Client 
{
    sealed class TakeDamageSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, DamageAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        

        public override MainEcsSystem Clone()
        {
            return new TakeDamageSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {   
                ref var damageComp = ref _takeDamagePool.Value.Get(entity);
                if(damageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var healthComp = ref _healthPool.Value.Get(targetEntity);

                    damageComp.Damage = healthComp.GetNewDamage(damageComp.Damage);
                    float healthValue = healthComp.TakeDamageReturnCurrent(damageComp.Damage);
                    
                    if (!_playerPool.Value.Has(targetEntity))
                    {
                        BattleState.Instance.AddDamage(damageComp.Damage);
                    }
                }
            }
        }
    }
}