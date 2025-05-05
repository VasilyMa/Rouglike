using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct TimerForResolveComponent : IConditionComponent
    {
        public AnimationCurve TimeToResolve;
        [HideInInspector] public float TimerToResolve;
        [SerializeReference] public List<IConditionResolve> Resolve;
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _timerForResolvePool = world.GetPool<TimerForResolveComponent>();
            if (!_timerForResolvePool.Has(entityCondition)) _timerForResolvePool.Add(entityCondition);
            ref var timerForResolveComp = ref _timerForResolvePool.Get(entityCondition);
            timerForResolveComp.Resolve = new(Resolve);
            timerForResolveComp.TimeToResolve = TimeToResolve;
            ref var pointConditionComp = ref world.GetPool<PointsConditionComponent>().Get(entityCondition);
            timerForResolveComp.TimerToResolve = TimeToResolve.Evaluate(pointConditionComp.CurrentPoints);
        }
    }
}