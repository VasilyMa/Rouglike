using Leopotam.EcsLite;
using UnityEngine;
using Statement;

namespace Client
{
    struct MissileComponent
    {
        public MissileMB missile;
        public float Speed;
        public Vector3 Offset;
        [HideInInspector] public string LayerNameTarget;
        public Vector3 CasterPosition;
        public Vector3 TargetPosition;
        public void Init()
        {

        }

        public void Invoke(int missileEntity, int entityCaster, int abilityEntity, EcsWorld world)
        {
            ref var missileComp = ref world.GetPool<MissileComponent>().Get(missileEntity);

            missileComp.missile = PoolModule.Instance.GetFromPool<MissileMB>(missile,true);

            missileComp.Speed = Speed;
            missileComp.Offset = Offset;
            ref var transformComp = ref world.GetPool<TransformComponent>().Add(missileEntity);
            Transform casterTransform = world.GetPool<TransformComponent>().Get(entityCaster).Transform;
            transformComp.Transform = missileComp.missile.transform;
            transformComp.Transform.position = casterTransform.TransformPoint(missileComp.Offset);
            transformComp.Transform.localEulerAngles = casterTransform.rotation.eulerAngles;
            missileComp.missile.gameObject.SetActive(true);
            ref var casterMissileComponent = ref world.GetPool<CasterMissileComponent>().Add(missileEntity);
            casterMissileComponent.EntityCaster = world.PackEntity(entityCaster);
            if (world.GetPool<PlayerComponent>().Has(entityCaster))
            {
                LayerNameTarget = "Enemy";
                ref var mouseComp = ref world.GetPool<MousePositionInputEvent>().Get(State.Instance.GetEntity("InputEntity"));
                TargetPosition = mouseComp.MousePosition + Offset;
                //if the player has the opportunity to throw misls at the enemy, then you can hang on him!
                //ref var targetMissileComp = ref world.GetPool<TargetMissileComponent>().Add(entityCaster);
                //targetMissileComp.EntityTarget = world.PackEntity(entityTarget);
            }
            if (world.GetPool<EnemyComponent>().Has(entityCaster))
            {
                LayerNameTarget = "Player";
                ref var targetComp = ref world.GetPool<TargetsContext>().Get(entityCaster);

                if (targetComp.closestEnemyEntity.Unpack(world, out int entityTarget))
                {
                    ref var transformTarget = ref world.GetPool<TransformComponent>().Get(entityTarget);
                    ref var targetMissileComp = ref world.GetPool<TargetMissileComponent>().Add(missileEntity);
                    targetMissileComp.EntityTarget = world.PackEntity(entityTarget);
                    TargetPosition = transformTarget.Transform.position;
                }
            }
        }
        public void ChangeViewMissile(MissileMB newMissile, int missileEntity, EcsWorld world)
        {
            var position = missile.transform.position;
            var angles = missile.transform.rotation;
            missile.gameObject.SetActive(false);
            missile.ReturnToPool();
            missile = PoolModule.Instance.GetFromPool<MissileMB>(newMissile, true);

            ref var transformComp = ref world.GetPool<TransformComponent>().Get(missileEntity);
            transformComp.Transform = missile.transform;
            transformComp.Transform.position = position;
            transformComp.Transform.rotation = angles;
            missile.gameObject.SetActive(true);
        }

        public void Dispose(int entityCaster, EcsWorld world)
        {

        }

    }
}