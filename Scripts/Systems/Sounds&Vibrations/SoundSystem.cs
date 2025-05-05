using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class SoundSystem : MainEcsSystem
    {
        private readonly EcsFilterInject<Inc<PlaySoundEvent>> _playSoundFilter = default;
        private readonly EcsPoolInject<PlaySoundEvent> _playSoundPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new SoundSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _playSoundFilter.Value)
            {
                ref var playSoundEvent = ref _playSoundPool.Value.Get(entity);
                if (playSoundEvent.SoundTransform)
                {
                    //SoundManager.Instance.PlayAudioAtPosition(playSoundEvent.eventReference, playSoundEvent.SoundTransform.position);
                    SoundEntity.Instance.PlayAudioAttached(playSoundEvent.eventReference, playSoundEvent.SoundTransform);
                }
                else
                {
                    ref var transformComp = ref _transformPool.Value.Get(playSoundEvent.entity);
                    SoundEntity.Instance.PlayAudioAttached(playSoundEvent.eventReference, transformComp.Transform);
                    //SoundManager.Instance.PlayAudioAtPosition(playSoundEvent.eventReference, transformComp.Transform.position);
                }
                _playSoundPool.Value.Del(entity);
            }
        }
    }
}
