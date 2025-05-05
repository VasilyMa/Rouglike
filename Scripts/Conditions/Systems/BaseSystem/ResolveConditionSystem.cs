using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ResolveConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ConditionCompnent, ResolveConditionEvent,PointsConditionComponent>> _filter;
        readonly EcsPoolInject<ResolveConditionEvent> resolvePool;
        readonly EcsPoolInject<ConditionCompnent> _ownerConditionPool;
        readonly EcsPoolInject<PointsConditionComponent> _pointsConditionPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new ResolveConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var resolveAfterPoint = ref resolvePool.Value.Get(entity);
                ref var ownerConditionComp = ref _ownerConditionPool.Value.Get(entity);
                ref var pointCondition = ref _pointsConditionPool.Value.Get(entity);
                if (!ownerConditionComp.PackedEntityOwner.Unpack(_world.Value, out int entityOwner)) continue;
                foreach (var component in resolveAfterPoint.resolve)
                {
                    component.Recalculate(pointCondition.CurrentPoints);
                    component.InvokeResolve(entity, entityOwner, _world.Value);
                }
            }
        }
    }
}