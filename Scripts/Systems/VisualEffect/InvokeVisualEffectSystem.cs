using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class InvokeVisualEffectSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InvokeVisualEffectEvent>> _filter = default;
        readonly EcsPoolInject<InvokeVisualEffectEvent> _invokePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool;

        public override MainEcsSystem Clone()
        {
            return new InvokeVisualEffectSystem();
        }

        public override void Run (IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var invokeComp = ref _invokePool.Value.Get(entity);
                if(invokeComp.EntityCaster.Unpack(_world.Value, out int entityCaster))
                {

                    SourceParticle sourceParticle = PoolModule.Instance.GetFromPool<SourceParticle>(invokeComp.Particle,true);
                    ref var phisicsComp = ref _world.Value.GetPool<PhysicsUnitComponent>().Get(entityCaster);
                    Vector3 worldOffset = phisicsComp.PhysicsUnitMB.transform.TransformDirection(invokeComp.offset);
                    Vector3 targetPosition = phisicsComp.PhysicsUnitMB.transform.position + worldOffset;
                    Quaternion targetRotate = phisicsComp.PhysicsUnitMB.transform.localRotation;
                    targetRotate = Quaternion.Euler(invokeComp.RotationOffset.x, targetRotate.eulerAngles.y + invokeComp.RotationOffset.y,invokeComp.RotationOffset.z);
                    Transform parent = invokeComp.IsParentTransformInvoke ? phisicsComp.PhysicsUnitMB.Transform : null; //todo POOL
                    sourceParticle.AttachVisualEffectToEntity(targetPosition, targetRotate, invokeComp.AbilityEntity, parent);

                }

            }
        }
    }
}