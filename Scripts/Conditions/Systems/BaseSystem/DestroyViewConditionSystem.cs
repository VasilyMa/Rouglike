using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DestroyViewConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<DestroyConditionEvent, ViewConditionComponent>> _filter;
        readonly EcsPoolInject<ViewConditionComponent> _viewConditionPool;

        public override MainEcsSystem Clone()
        {
            return new DestroyViewConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var viewConditionComp = ref _viewConditionPool.Value.Get(entity);
                if (viewConditionComp.SourseParticle is null) continue;
                viewConditionComp.SourseParticle.Dispose();
            }
        }
    }
}