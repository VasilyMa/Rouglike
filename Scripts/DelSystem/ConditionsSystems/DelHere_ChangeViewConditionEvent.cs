using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereChangeViewConditionEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ChangeViewConditionEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereChangeViewConditionEvent();
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
