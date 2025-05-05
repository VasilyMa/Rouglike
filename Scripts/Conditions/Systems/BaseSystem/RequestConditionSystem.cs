using FMOD;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;

namespace Client {
    sealed class RequestConditionSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestConditionEvent>> _filter;
        readonly EcsPoolInject<RequestConditionEvent> _requestCondtionPool;
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<AddPointConditionEvent> _addPointConditionPool;
        readonly EcsPoolInject<RequestConditionOverlayEvent> _rerquestConditionOverlayPool;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool;
        public override MainEcsSystem Clone()
        {
            return new RequestConditionSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestConditionComp = ref _requestCondtionPool.Value.Get(entity);
                if (!requestConditionComp.entityTarget.Unpack(_world.Value, out int entityTarget)) continue;
                if (_momentDeadPool.Value.Has(entityTarget)) continue;
                if (!_conditionContainerPool.Value.Has(entityTarget)) continue;
                ref var conditionContainerComp = ref _conditionContainerPool.Value.Get(entityTarget);
                if(conditionContainerComp.Conditions.ContainsKey(requestConditionComp.Condition))
                {
                    if (!conditionContainerComp.Conditions[requestConditionComp.Condition].Unpack(_world.Value, out int entityCondition)) continue;
                    if (!_addPointConditionPool.Value.Has(entityCondition)) _addPointConditionPool.Value.Add(entityCondition).CountPoint = 0;
                    ref var addPointConditioncComp = ref _addPointConditionPool.Value.Get(entityCondition);
                    addPointConditioncComp.CountPoint += requestConditionComp.ExtensionsPointPoint;
                }
                else
                {
                    ref var requestConditiondOverlayComp = ref _rerquestConditionOverlayPool.Value.Add(_world.Value.NewEntity());
                    requestConditiondOverlayComp.StartCountPoint = requestConditionComp.ExtensionsPointPoint;
                    requestConditiondOverlayComp.Condition = requestConditionComp.Condition;
                    requestConditiondOverlayComp.OwnerEntity = requestConditionComp.entityTarget;
                }
            }
        }
    }
}