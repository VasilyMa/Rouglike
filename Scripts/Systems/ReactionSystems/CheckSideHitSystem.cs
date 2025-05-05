using FMOD;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class CheckSideHitSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<TakeDamageComponent, HitAnimationAllowedComponent>, Exc<CheckSideHitEvent>> _filter = default;
        private readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<CheckSideHitEvent> _checkSidePool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckSideHitSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);

                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if (takeDamageComp.KillerEntity.Unpack(_world.Value, out int killerEntity))
                    {
                        ref var transformTarget = ref _transformPool.Value.Get(targetEntity);
                        ref var transformSender = ref _transformPool.Value.Get(killerEntity);
                        if (transformTarget.Transform)
                        {
                            Vector3 attackDirection = (transformTarget.Transform.position - transformSender.Transform.position).normalized;
                            attackDirection.y = 0f;

                            Vector3 localDirection = Quaternion.Inverse(transformTarget.Transform.rotation) * attackDirection;

                            float angle = Mathf.Atan2(localDirection.z, localDirection.x) * Mathf.Rad2Deg;

                            ref var chechSideEvent = ref _checkSidePool.Value.Add(entity);
                            chechSideEvent.Angle = angle;
                        }
                    }
                }   
                
            }
        }
    }
}