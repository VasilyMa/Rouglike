using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Client 
{
    struct KnockbackEffect : IAbilityEffect
    {
        [HideInInspector]public float Duration;
        EcsPool<KnockbackEffect> _pool;
        [HideInInspector]public bool IsGetUp;
        [HideInInspector]public bool IsKnocked;
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float DurationValue;
        [HideInInspector] public float GetUpTimer;

        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            _pool = world.GetPool<KnockbackEffect>();
            if (!_pool.Has(entity)) _pool.Add(entity);
                ref var poolComp = ref _pool.Get(entity);
                if(poolComp.Duration < Duration)
                {
                    poolComp.Duration = Duration;
                }
                poolComp.IsKnocked = false;
                poolComp.IsGetUp = true;
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