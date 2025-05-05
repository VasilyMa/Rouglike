using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereRemoveDelayRecoveryToughnessEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RemoveDelayRecoveryToughnessEvent>> _filter = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DelHereRemoveDelayRecoveryToughnessEvent();
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