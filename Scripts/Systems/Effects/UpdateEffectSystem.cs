using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class UpdateEffectSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<EffectsContainer>> _filter = default;
        readonly EcsPoolInject<EffectsContainer> _effectContainer = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateEffectSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var effectContainer = ref _effectContainer.Value.Get(entity);

                effectContainer.Run();
            }
        }
    }
}