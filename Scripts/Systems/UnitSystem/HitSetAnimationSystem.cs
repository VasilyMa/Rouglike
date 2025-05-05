using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class HitSetAnimationSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, CheckSideHitEvent, HitAnimationAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<HitAnimationState> _hitAnimationPool = default;
        private readonly EcsPoolInject<CheckSideHitEvent> _checkSideEventPool = default;
        public override MainEcsSystem Clone()
        {
            return new HitSetAnimationSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                ref var checkSideEvent = ref _checkSideEventPool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    SetHitAnimation(targetEntity, ref checkSideEvent);
                }
            }
        }
        private void SetHitAnimation(int entity, ref CheckSideHitEvent checkSideEvent)
        {
            if (_hitAnimationPool.Value.Has(entity)) return;
            if (checkSideEvent.Angle >= -45f && checkSideEvent.Angle < 45f)
                _hitAnimationPool.Value.Add(entity).Type = HitAnimationType.GetHitLeft;
            else if (checkSideEvent.Angle >= 45f && checkSideEvent.Angle < 135f)
                _hitAnimationPool.Value.Add(entity).Type = HitAnimationType.GetHitBack;
            else if (checkSideEvent.Angle >= -135f && checkSideEvent.Angle < -45f)
                _hitAnimationPool.Value.Add(entity).Type = HitAnimationType.GetHitFront;
            else
                _hitAnimationPool.Value.Add(entity).Type = HitAnimationType.GetHitRight;
        }
    }
}