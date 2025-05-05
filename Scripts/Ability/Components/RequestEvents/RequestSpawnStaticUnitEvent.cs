using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct RequestSpawnStaticUnitEvent : IAbilityComponent
    {
        public GameObject spawnObject;
        public bool delDesynchronization;
        public bool isImmortal;
        [HideIf("isImmortal", true)] public float Health;
        public float MinRange;
        public float MaxRange;
        public AIProfile AIprofile;
        public List<AbilityBase> ability;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {

        }

        public void Init()
        {

        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            var spawnPool = world.GetPool<SpawnStaticUnitEvent>();
            var transformPool = world.GetPool<TransformComponent>();
            ref var transformOwner = ref transformPool.Get(ownerEntity);
            ref var spawnEvent = ref spawnPool.Add(world.NewEntity());
            spawnEvent.position = RandomPointGenerator.GetRandomPoint(transformOwner.Transform.position, MinRange, MaxRange);
            spawnEvent.Health = Health;
            spawnEvent.OwnnerEntity = world.PackEntity(ownerEntity);
            spawnEvent.isImmortal = isImmortal;
            spawnEvent.GameObject = spawnObject;
            spawnEvent.abilities = new(ability);
            spawnEvent.AIprofile = AIprofile;
            spawnEvent.delDesynchronization = delDesynchronization;
        }
    }
}