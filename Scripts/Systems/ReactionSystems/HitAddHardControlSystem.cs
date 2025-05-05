using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class HitAddHardControlSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<TakeDamageComponent, HitAnimationAllowedComponent, CheckSideHitEvent>> _filter = default;
        readonly EcsPoolInject<RequestAddHardControlEvent> _requestHardControlPool = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        public override MainEcsSystem Clone()
        {
            return new HitAddHardControlSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var requestComp = ref _requestHardControlPool.Value.Add(_world.Value.NewEntity());
                    requestComp.TargetEntity = takeDamageComp.TargetEntity;
                    requestComp.TheTimerShouldBeSetToTheTimeUntilTheEndOfTheAnimation = true;
                }
                
            }
        }
    }
}