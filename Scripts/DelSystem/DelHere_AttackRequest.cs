using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereAttackRequest: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AttackRequest>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereAttackRequest();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}