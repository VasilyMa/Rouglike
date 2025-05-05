using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DelHere_AnimationStateComponent : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AnimationStateComponent>> _filter = default;
        
        public override MainEcsSystem Clone()
        {
            return new DelHere_AnimationStateComponent();
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