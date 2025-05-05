using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHereKnockbackAnimationState: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<KnockbackAnimationState>> _filter = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DelHereKnockbackAnimationState();
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