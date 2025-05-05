using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereMoveToTargetRequest: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MoveToTargetRequest>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereMoveToTargetRequest();
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