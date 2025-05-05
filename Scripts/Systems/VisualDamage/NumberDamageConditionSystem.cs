using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class NumberDamageConditionSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, NumberDamageAllowedComponent, ConditionTakeDamageComponent>> _filter;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        private VisualDamageConfig _visualDamageConfig;
        private Vector3 _damageNumberOffset = new Vector3(0f, 2f, 0f); // offset to damage numbers at Y-axis

        //todo PODSKAL DAMAGE NUMBER AFTER CONDITION

        public override MainEcsSystem Clone()
        {
            return new NumberDamageConditionSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            _visualDamageConfig = ConfigModule.GetConfig<ViewConfig>().VisualDamageConfig;
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var transfromComp = ref _transformPool.Value.Get(targetEntity);
                
                    var positionSpawn = transfromComp.Transform.position + _damageNumberOffset;
                    if(!_playerPool.Value.Has(targetEntity))
                    {
                        _visualDamageConfig.ConditionDamageEnemy.Spawn(positionSpawn, takeDamageComp.Damage);
                    }
                    else
                    {
                        _visualDamageConfig.ConditionDamagePlayer.Spawn(positionSpawn, takeDamageComp.Damage);
                    }
                }
            }
        }
    }
}