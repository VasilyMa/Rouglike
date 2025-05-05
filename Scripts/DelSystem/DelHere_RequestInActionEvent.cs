using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_RequestInActionEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestInActionEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_RequestInActionEvent();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}