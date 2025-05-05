using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TMPro;
using UnityEngine;

namespace Client
{
    public class CalculationOfTimeIntervalsSystem: MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CalculationHitIntervalEvent>> _filter;
        readonly EcsPoolInject<CalculationHitIntervalEvent> _calculationHitIntervalPool;
        readonly EcsPoolInject<HardHitComponent> _hardHitPool;
        readonly EcsPoolInject<HitAnimationState> _hitAnimationPool;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool;
        readonly EcsPoolInject<TimerBeforeApprovedDashComponent> _timerBeforeApprovedDashPool;
        readonly EcsWorldInject _world;
        private float TimeAnimationHit = 0;
        private float PartOneHit = 0;
        private float PartTwoHit = 0;
        public override void Init(IEcsSystems systems)
        {
            var GameConfig = ConfigModule.GetConfig<GameConfig>();
            PartOneHit = GameConfig.PartOneHit;
            PartTwoHit = GameConfig.PartTwoHit;
            TimeAnimationHit = GameConfig.TimeAnimationHit;
        }

        public override MainEcsSystem Clone()
        {
            return new CalculationOfTimeIntervalsSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var addHitIntervalComp = ref _calculationHitIntervalPool.Value.Get(entity);
                ref var hardHitComp = ref _hardHitPool.Value.Add(entity);
                hardHitComp.TimerHardHit = TimeAnimationHit * (PartOneHit + PartTwoHit);
                 ref var timerBeforeApprovedComp = ref _timerBeforeApprovedDashPool.Value.Add(entity);
                timerBeforeApprovedComp.TimerBeforApproved = TimeAnimationHit * PartOneHit;
                timerBeforeApprovedComp.TimeApprovedDash = TimeAnimationHit * (1 - PartOneHit);
            }

        }
        public string GetNameAnimationClip(HitAnimationType hitState)
        {
            if (hitState == HitAnimationType.GetHitRight) return "react_right";
            if (hitState == HitAnimationType.GetHitLeft) return "react_left";
            if (hitState == HitAnimationType.GetHitBack) return "react_back";
            if (hitState == HitAnimationType.GetHitFront) return "react_front";
            return string.Empty;
        }
    }
}