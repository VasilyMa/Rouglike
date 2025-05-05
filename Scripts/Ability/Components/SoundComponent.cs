using AbilitySystem;
using FMODUnity;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    struct SoundComponent : IAbilityComponent
    {
        [SerializeField] EventReference eventReference;
        public bool RandomSound;
        EcsPool<PlaySoundEvent> _pool;
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {

        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            _pool = world.GetPool<PlaySoundEvent>();
            ref var poolComp = ref _pool.Add(world.NewEntity());
            poolComp.eventReference = eventReference;
            poolComp.entity = entityCaster;
        }
    }
}