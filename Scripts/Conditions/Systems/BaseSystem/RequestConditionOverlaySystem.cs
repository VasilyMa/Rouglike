using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;

namespace Client {
    sealed class RequestConditionOverlaySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestConditionOverlayEvent>> _filter;
        readonly EcsPoolInject<RequestConditionOverlayEvent> _requestConditionOverlayPool;
        readonly EcsPoolInject<ResistanceConditionComponent> _resistansConditionPool;
        readonly EcsPoolInject<ConditionOverlayToUnitEvent> _conditionOverlayToUnitPool;
        readonly EcsWorldInject _world;
        private ConditionConfig _conditionConfig;
        public override MainEcsSystem Clone()
        {
            return new RequestConditionOverlaySystem();
        }

        public override void Init(IEcsSystems systems)
        {
            _conditionConfig = ConfigModule.GetConfig<ConditionConfig>();//������������ �� ������ �����
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestConditionsOverlayComp = ref _requestConditionOverlayPool.Value.Get(entity);
                if (!requestConditionsOverlayComp.OwnerEntity.Unpack(_world.Value, out int entityOwner)) continue;
                if (!_resistansConditionPool.Value.Has(entityOwner)) continue;
                ref var resistCondition = ref _resistansConditionPool.Value.Get(entityOwner);
                if (resistCondition.resistConditions.Contains(requestConditionsOverlayComp.Condition)) continue;
                var condition = requestConditionsOverlayComp.Condition;
                var conditionSettings = _conditionConfig.ConditionData.FirstOrDefault(x => x._condition == condition);
                if (conditionSettings is null) continue;
                ref var conditionOverlayToUnit = ref _conditionOverlayToUnitPool.Value.Add(entity);
                conditionOverlayToUnit.ConditionSettings = conditionSettings;

            }
        }
    }
}