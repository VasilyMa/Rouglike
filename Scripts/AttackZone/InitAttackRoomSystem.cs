using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class InitAttackRoomSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<InitAttackRoomEvent, AttackRoomComponent>> _filter;
        readonly EcsPoolInject<InitAttackRoomEvent> _initAttackPool;
        readonly EcsPoolInject<TimerAttackRoomComponent> _timerAttackRoomComponent;
        readonly EcsPoolInject<ResolveBlockComponent> _resolveBlockPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitAttackRoomSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var initAttackComp = ref _initAttackPool.Value.Get(entity);
                ref var timerAttackComp = ref _timerAttackRoomComponent.Value.Add(entity);
                timerAttackComp.TimerForResolve = initAttackComp.TimeToResolve;
                timerAttackComp.TimeToResolve = initAttackComp.TimeToResolve;
                timerAttackComp.LifeTime = initAttackComp.LifeTime;
                timerAttackComp.Delay = initAttackComp.Delay;
                ref var resolveBlockComp = ref _resolveBlockPool.Value.Add(entity);
                resolveBlockComp.Components = new(initAttackComp.Components);
                foreach (var component in resolveBlockComp.Components)
                {
                    component.Recalculate(1f);
                }
                _initAttackPool.Value.Del(entity);
            }
        }
    }
}