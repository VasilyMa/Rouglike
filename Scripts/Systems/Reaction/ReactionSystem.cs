using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    sealed class ReactionSystem : IEcsRunSystem
    {
        
        private DG.Tweening.Sequence seq;
        private const float REDUCTION_VALUE = 100;
        public void Run(IEcsSystems systems)
        {
            //foreach (var entity in _filter.Value)
            //{
            //    ref var reactionEvent = ref _pool.Value.Get(entity);
            //    ref var viewOwner = ref _viewPool.Value.Get(entity);
            //    ref var viewSender = ref _viewPool.Value.Get(reactionEvent.EntitySender);

            //    //float toughness = float.MinValue;//прочность
            //    //if(_toughnessPool.Value.Has(entity))
            //    //{
            //    //    ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
            //    //    toughness = toughnessComp.CurrentValue;
            //    //}

            //    //if(_irrevocabilityPool.Value.Has(entity)) continue;//?
            //    //if(toughness > 0) continue;//?
            //    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    var agent = viewOwner.Transform.gameObject.GetComponent<NavMeshAgent>();//add NavMesh to view//Зачем тут нужен

            //    Vector3 attackDirection = (viewOwner.Transform.position - viewSender.Transform.position).normalized;
            //    attackDirection.y = 0f;

            //    Vector3 localDirection = Quaternion.Inverse(viewOwner.Transform.rotation) * attackDirection;

            //    float angle = Mathf.Atan2(localDirection.z, localDirection.x) * Mathf.Rad2Deg;
            //    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    var dist = CheckPushDistance(viewOwner.Transform.position, attackDirection, reactionEvent.PushForce/ REDUCTION_VALUE / viewOwner.UnitMB.Mass);
            //    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    //

            //    if (viewOwner.Transform)
            //    {
            //        viewOwner.Transform.DOMove(viewOwner.Transform.position + attackDirection * dist, 0.5f)
            //        .SetEase(Ease.OutCubic).OnComplete(() =>{ seq.Kill(); });

            //        if (viewSender.UnitMB is not null)
            //        {
            //            //to do hit effect
            //            //GameObject.Instantiate(viewSender.UnitMB.WeaponConfig.hitVisualEffects[0], (viewSender.Transform.position - viewOwner.Transform.position).normalized * viewOwner.UnitMB.HitOffset + viewOwner.Transform.position + Vector3.up, Quaternion.identity); // �������� �� �������� ������ + Offset
            //        }
            //    }
               
            //    if (viewOwner.UnitMB is not null)
            //    {
            //        viewOwner.UnitMB.DeactiveTeleGO();
            //        viewOwner.UnitMB.Dispose();
            //    }
            //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    AnimationTypes damageAnimationType = GetDamageAnimationType(angle);
            //    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    if (!_lockMovePool.Value.Has(entity)) _lockMovePool.Value.Add(entity);
            //    if (!_lockRotationPool.Value.Has(entity)) _lockRotationPool.Value.Add(entity);
            //    if (!_lockPool.Value.Has(entity)) _lockPool.Value.Add(entity);

            //    if (reactionEvent.IsAnimationInvoke)
            //    {
            //        ChangeAnimationController.ChangeAnimationFunc(damageAnimationType, entity);
            //    }
            //    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    AnimationTypes GetDamageAnimationType(float angle)
            //    {
            //        //todo SANIA ANIMATION
            //        if (angle >= -45f && angle < 45f)
            //        {
            //            
            //            return AnimationTypes.GetHitLeft;//!!!
            //        }
            //        else if (angle >= 45f && angle < 135f)
            //        {
            //            
            //            return AnimationTypes.GetHitBack;//!!!
            //        }
            //        else if (angle >= -135f && angle < -45f)
            //        {
            //            
            //            return AnimationTypes.GetHit;//!!!
            //        }
            //        else
            //        {
            //            
            //            return AnimationTypes.GetHitRight;//!!!
            //        }
            //    }

            //    _pool.Value.Del(entity);    
            //}
        }



        private float CheckPushDistance(Vector3 startPosition, Vector3 direction, float maxDistance)
        {

            RaycastHit hit;
            int layerMaskOnlyWall = 1 << 9;
            if (Physics.Raycast(startPosition, direction, out hit, maxDistance, layerMaskOnlyWall))
            {

                if (hit.distance < 1f)
                    hit.distance = 0;
                else hit.distance -= 1f;
                return hit.distance;
            }

            return maxDistance;
        }
    }
}
