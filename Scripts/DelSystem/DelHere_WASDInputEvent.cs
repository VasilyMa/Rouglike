using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

using UnityEngine;

namespace Client
{
    public class DelHereWASDInputEvent: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<WASDInputEvent>> _filter = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;

        public override MainEcsSystem Clone()
        {
            return new DelHereWASDInputEvent();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                _filter.Pools.Inc1.Del(entity);

                var playerEntity = State.Instance.GetEntity("PlayerEntity");

                ref var animatorComp = ref _animatorPool.Value.Get(playerEntity);
                animatorComp.Animator.SetBool("IsInput", false);
            }
        }
    }
}
