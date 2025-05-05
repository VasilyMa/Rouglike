using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SoundDelaySystem : IEcsRunSystem {

        private readonly EcsFilterInject<Inc<SoundComponent>> _soundComponentFilter = default;
        private readonly EcsFilterInject<Inc<SoundEffect>> _soundEffectFilter = default;
        private readonly EcsPoolInject<SoundComponent> _soundComponentPool = default;   
        private readonly EcsPoolInject<SoundEffect> _soundEffectPool = default;   
        public void Run (IEcsSystems systems) {
            foreach (var entity in _soundComponentFilter.Value)
            {
                ref var soundComponent = ref _soundComponentPool.Value.Get(entity);
            }
            foreach (var entity in _soundEffectFilter.Value)
            {
                ref var soundEffect = ref _soundEffectPool.Value.Get(entity);
            }
        }
    }
}