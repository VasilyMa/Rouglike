using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class NotDisposeSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<NotDispose>, Exc<UnusedHelper>> _filter;
        readonly EcsPoolInject<UnusedHelper> _unusedPool;
        public override MainEcsSystem Clone()
        {
            return new NotDisposeSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _unusedPool.Value.Add(entity);
            }
        }
    }
}