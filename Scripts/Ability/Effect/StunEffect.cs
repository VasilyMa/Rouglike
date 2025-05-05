using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Client {
    struct StunEffect : IAbilityEffect
    {
        [HideInInspector]public float Duration;
        public SourceParticle SourceParticle;
        [HideInInspector]public GameObject InstantiatedObject;
        [HideInInspector]public bool IsStuned;
        EcsPool<StunEffect> _pool;
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float DurationValue;
        // public VisualEffect VisualEffect;
        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            if (world.GetPool<BossComponent>().Has(entity)) return;
            _pool = world.GetPool<StunEffect>();
            if (!_pool.Has(entity)) _pool.Add(entity);
            ref var poolComp = ref _pool.Get(entity);
            if(poolComp.Duration < Duration)
            {
                poolComp.Duration = Duration;
            }
            
            poolComp.SourceParticle = SourceParticle;
            poolComp.IsStuned = IsStuned;
            var disposeHitIntervalPool = world.GetPool<DisposeHitIntervalEvent>();
            if (!disposeHitIntervalPool.Has(entity)) disposeHitIntervalPool.Add(entity);
        }

        public void Recalculate(float charge)
        {
            if(UsageValue == UsageValues.Curve)
            {
                Duration = CurveValue.Evaluate(charge);
            }
            else
            {
                Duration = DurationValue;
            }
        }
    }
}