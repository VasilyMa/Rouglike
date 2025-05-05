using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    sealed class ExternalMoveSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ExternalMoveComponent>,Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<RigidbodyComponent> _rbPool = default;
        readonly EcsPoolInject<ExternalMoveComponent> _externalMovePool = default;
        readonly EcsPoolInject<AnimatorComponent> _animPool = default;

        public override MainEcsSystem Clone()
        {
            return new ExternalMoveSystem();
        }

        public override void Run(IEcsSystems systems)
        {

            foreach (var entity in _filter.Value)
            {

                ref var externalMoveComp = ref _externalMovePool.Value.Get(entity);
                ref var rbComp = ref _rbPool.Value.Get(entity);
                ref var animComp = ref _animPool.Value.Get(entity);
                //animComp.Animator.applyRootMotion = false;

                Move(ref rbComp, ref externalMoveComp);

                if (externalMoveComp.Duration > 0)
                    externalMoveComp.Duration -= Time.fixedDeltaTime;
                else if (externalMoveComp.Duration <= 0)
                    _externalMovePool.Value.Del(entity);
            }
        }
        private void Move(ref RigidbodyComponent rbComp, ref ExternalMoveComponent externalMoveComp)
        {
            if (externalMoveComp.Direction != RequestExternalMoveEvent.DirectionType.ToTarget)
            {
                rbComp.Rigidbody.AddForce(((externalMoveComp.MoveDirection * externalMoveComp.Speed) + externalMoveComp.SupportDirection)*Time.timeScale, externalMoveComp.ForceMode);
                rbComp.Rigidbody.drag = 10;
            }
            else
            {
                var targetPos = rbComp.Rigidbody.transform.position + externalMoveComp.MoveDirection;
                if (NavMesh.SamplePosition(targetPos, out var hit, 10, NavMesh.AllAreas))
                {
                    rbComp.Rigidbody.transform.position = Vector3.Lerp(rbComp.Rigidbody.transform.position, hit.position, Time.fixedDeltaTime);
                    targetPos = hit.position;
                }
            }
        }
    }
}