using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PushAfterRagDollSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<DeadComponent,ExternalMoveComponent,TelegraphingUnitComponent>> _filter;
        readonly EcsPoolInject<DeadComponent> _deadComponent;
        readonly EcsPoolInject<ExternalMoveComponent> _externalMovePool;
        readonly EcsPoolInject<TelegraphingUnitComponent> _telegraphingUnitPool;
        private float PUSH_MULTIPLY;
        public override void Init(IEcsSystems systems)
        {
            PUSH_MULTIPLY = ConfigModule.GetConfig<GameConfig>().PushForce;
        }
        public override MainEcsSystem Clone()
        {
            return new PushAfterRagDollSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var deadComponent = ref _deadComponent.Value.Get(entity);
                ref var externalMoveComp = ref _externalMovePool.Value.Get(entity);
                ref var telegraphingComp = ref _telegraphingUnitPool.Value.Get(entity);

                if (externalMoveComp.Duration > 0)
                    externalMoveComp.Duration -= Time.fixedDeltaTime;
                else if (externalMoveComp.Duration <= 0)
                    _externalMovePool.Value.Del(entity);

                telegraphingComp.TelegraphingUnitMB.PushForceBody(((externalMoveComp.MoveDirection * externalMoveComp.Speed * PUSH_MULTIPLY) + externalMoveComp.SupportDirection) * Time.timeScale, externalMoveComp.ForceMode);
            }
        }
    }
}