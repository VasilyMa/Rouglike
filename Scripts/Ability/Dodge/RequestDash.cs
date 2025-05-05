using AbilitySystem;
using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct RequestDash : IAbilityComponent
    { 
        public float Duration;
        public float AnimationSpeed;
        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
            
        }

        public void Init()
        {
        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<RequestDash> pool = world.GetPool<RequestDash>();
            if (!pool.Has(entityCaster)) pool.Add(entityCaster);
            ref var speedAnimComp = ref pool.Get(entityCaster);
            speedAnimComp.Duration = Duration;
            speedAnimComp.AnimationSpeed = AnimationSpeed;
        }
    }
    struct TimeStopMoveComponent
    {
        public Vector3 EndPoint;
        public Ease Boost;
        public AnimationTypes AnimationType;
    }
    struct MoveEvent
    {
        public float TimeMove;
        public Vector3 EndPoint;
        public Ease Boost;
    }
    struct MoveAbilityComponent
    {
        public Tween Tween;
        public Vector3 endPosition;
        public Ease Boost;
    }
}