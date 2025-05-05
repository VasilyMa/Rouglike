using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_RequestAttackRoomEvent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestAttackRoomEvent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_RequestAttackRoomEvent();
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