using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UtilityAbilityButtonDownSystem : IEcsRunSystem { 
        readonly EcsFilterInject<Inc<UnitComponent, UtilityAbilityButtonDownEvent>, Exc<ActionComponent>> _filter = default;
        readonly EcsPoolInject<ActionComponent> _actionPool = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var actionComp = ref _actionPool.Value.Add(entity);
                actionComp.ActionType = ActionTypes.UtilityAbilityDown;
            }
        }
    }
}