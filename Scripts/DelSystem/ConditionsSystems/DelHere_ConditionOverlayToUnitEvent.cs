using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereConditionOverlayToUnitEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ConditionOverlayToUnitEvent>> _filter = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereConditionOverlayToUnitEvent();
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
