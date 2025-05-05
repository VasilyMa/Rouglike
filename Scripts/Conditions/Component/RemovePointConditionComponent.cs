using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    struct RemovePointConditionComponent : IConditionComponent
    {
        public bool allPointRemove;
        [HideIf("allPointRemove")] public int CountPoint;
        public AnimationCurve TimeToRemove;
        [HideInInspector] public float TimerToRemove;
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _removePointPool = world.GetPool<RemovePointConditionComponent>();
            if (!_removePointPool.Has(entityCondition)) _removePointPool.Add(entityCondition);
            ref var removePointComp = ref _removePointPool.Get(entityCondition);
            removePointComp.allPointRemove = allPointRemove;
            removePointComp.CountPoint = CountPoint;
            removePointComp.TimeToRemove = TimeToRemove;
            ref var pointsConditionComp = ref world.GetPool<PointsConditionComponent>().Get(entityCondition);
            removePointComp.TimerToRemove = TimeToRemove.Evaluate(pointsConditionComp.CurrentPoints);
        }
    }
}