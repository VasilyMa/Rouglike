using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereGlobalMapGenerateCompleteEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<GlobalMapGenerateCompleteEvent>> _filter = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DelHereGlobalMapGenerateCompleteEvent();
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