using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereCreateEnemyEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CreateEnemyEvent>> _filter = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DelHereCreateEnemyEvent();
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