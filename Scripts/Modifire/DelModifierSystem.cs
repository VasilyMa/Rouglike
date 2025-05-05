using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class DelModifierSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestDelModifier>> _filter = default;
        readonly EcsPoolInject<RequestDelModifier> _delPool = default;
        readonly EcsPoolInject<ModifiersContainer> _containerPool = default;
        readonly EcsPoolInject<ModifiersContainerChangesEvent> _changePool = default;
        public override MainEcsSystem Clone()
        {
            return new DelModifierSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var delComp = ref _delPool.Value.Get(entity);
                if(delComp.UnitPackedEntity.Unpack(_world.Value, out int unitEntity))
                {
                    ref var containerComp = ref _containerPool.Value.Get(unitEntity);
                    
                    containerComp.Modifiers.Remove(delComp.Modifier);
                    if(!_changePool.Value.Has(unitEntity)) _changePool.Value.Add(unitEntity);
                }
            }
        }
    }
}