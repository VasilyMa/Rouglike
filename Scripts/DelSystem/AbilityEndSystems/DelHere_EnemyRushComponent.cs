using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereEnemyRushComponent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<EnemyRushComponent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereEnemyRushComponent();
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
