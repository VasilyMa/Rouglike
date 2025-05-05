using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Statement;
using static UnityEngine.EventSystems.EventTrigger;

namespace Client {
    struct RequestSpawnUnitEvent : IAbilityComponent, IAbilityMissileComponent
    {
        public UnitMeta Unit;
        public int CountUnit;
        public bool inRandomPoint;
        [ShowIf("inRandomPoint")]
        public float MinRange;
        [ShowIf("inRandomPoint")]
        public float MaxRange;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            SpawnUnit(world, ownerEntity);
        }
        public void Invoke(int entity,EcsWorld world, float charge)
        {
            SpawnUnit(world,entity);
            world.GetPool<FinishMissileEvent>().Add(entity);
        }
        public void SpawnUnit(EcsWorld world, int ownerEntity)
        {
            EcsPool<SpawnUnitWithDelay> poolSpawn = world.GetPool<SpawnUnitWithDelay>();
            EcsPool<TransformComponent> poolTransform = world.GetPool<TransformComponent>();
            BattleState.Instance.CurrentRoom.CurrentNumberOfEnemies += CountUnit;
            if (!inRandomPoint)
            {
                for (int i = 0; i < CountUnit; i++)
                {
                    ref var spawnComp = ref poolSpawn.Add(world.NewEntity());
                    spawnComp.SpawnPos = BattleState.Instance.CurrentRoom.SpawnPoints[Random.Range(0, BattleState.Instance.CurrentRoom.SpawnPoints.Length)].position;
                    spawnComp.UnitConfig = Unit.GetUnit();
                    spawnComp.EnemyMetaConfig = Unit.enemyMetaDataConfig;
                }
            }
            else
            {
                for (int i = 0; i < CountUnit; i++)
                {
                    ref var spawnComp = ref poolSpawn.Add(world.NewEntity());
                    ref var transfromComp = ref poolTransform.Get(ownerEntity);
                    spawnComp.SpawnPos = RandomPointGenerator.GetRandomPoint(transfromComp.Transform.position, MinRange, MaxRange);
                    spawnComp.UnitConfig = Unit.GetUnit();
                    spawnComp.EnemyMetaConfig = Unit.enemyMetaDataConfig;
                }
            }
        }
    }
}