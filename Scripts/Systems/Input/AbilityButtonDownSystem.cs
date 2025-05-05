using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AbilityButtonDownSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<UnitComponent, AbilityButtonDownEvent>, Exc<ActionComponent>> _filter = default;
        readonly EcsPoolInject<ActionComponent> _actionPool = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var actionComp = ref _actionPool.Value.Add(entity);
                actionComp.ActionType = ActionTypes.ProjectTileAbilityDown;
            }
        }
    }
}