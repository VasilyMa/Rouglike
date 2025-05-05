using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisableVisualEffectSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<VisualEffectsComponent, MomentDeadEvent>> _filter;
        readonly EcsPoolInject<VisualEffectsComponent> _visualPool = default;

        public override MainEcsSystem Clone()
        {
            return new DisableVisualEffectSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var visualComp = ref _visualPool.Value.Get(entity);
                foreach (var effect in visualComp.SourceParticles)
                {
                    effect.Dispose();
                }
                visualComp.SourceParticles.Clear();
            }
        }
    }
}