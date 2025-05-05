using AbilitySystem;
using FMODUnity;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    struct SoundEffect : IAbilityEffect
    {
        [SerializeField] EventReference SoundBundle;
        public bool RandomSound;
        EcsPool<PlaySoundEvent> _pool;
        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            _pool = world.GetPool<PlaySoundEvent>();
            ref var poolComp = ref _pool.Add(world.NewEntity());
            poolComp.eventReference = SoundBundle;
            poolComp.entitySender = entitySender;
            poolComp.entity = entity;
        }

        public void Recalculate(float charge)
        {
            
        }
    }
}