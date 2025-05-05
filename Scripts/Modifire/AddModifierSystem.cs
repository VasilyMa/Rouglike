using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class AddModifierSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<RequestAddModifier>> _filter = default;
        readonly EcsPoolInject<RequestAddModifier> _addPool = default;
        readonly EcsPoolInject<ModifiersContainer> _containerPool = default;
        readonly EcsPoolInject<ModifiersContainerChangesEvent> _changePool = default;
        public override MainEcsSystem Clone()
        {
            return new AddModifierSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var addComp = ref _addPool.Value.Get(entity);
                if(addComp.UnitPackedEntity.Unpack(_world.Value, out int unitEntity))
                {
                    ref var containerComp = ref _containerPool.Value.Get(unitEntity);
                    containerComp.Modifiers.Add(addComp.Modifier);
                    if(!_changePool.Value.Has(unitEntity)) _changePool.Value.Add(unitEntity);
                }
            }
        }
        
    }
}