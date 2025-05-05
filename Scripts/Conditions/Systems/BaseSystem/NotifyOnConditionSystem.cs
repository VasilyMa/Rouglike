using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Sirenix.OdinInspector;

namespace Client {
    sealed class NotifyOnConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<NotifyOnConditionEvent>> _filter;
        readonly EcsPoolInject<NotifyOnConditionEvent> _notifyOnConditionPool;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        readonly EcsPoolInject<ListenOnConditionEvent> _listenOnConditionPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new NotifyOnConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var notifyOnConditionComp = ref _notifyOnConditionPool.Value.Get(entity);
                if (!notifyOnConditionComp.OwnerEntity.Unpack(_world.Value, out int ownerCondition)) continue;
                if (!_conditionContainerPool.Value.Has(ownerCondition)) continue;
                ref var conditionContainerComp = ref _conditionContainerPool.Value.Get(ownerCondition);
                if (!conditionContainerComp.Conditions.TryGetValue(notifyOnConditionComp.RecipientCondition, out var recipientEntity))
                {
                    _notifyOnConditionPool.Value.Del(entity);
                    continue;
                }
                if (!recipientEntity.Unpack(_world.Value, out int recipientEntityUnpack)) continue;
                if (!_listenOnConditionPool.Value.Has(recipientEntityUnpack)) _listenOnConditionPool.Value.Add(recipientEntityUnpack).ConditionSenders = new();
                ref var listenOnConditionComp = ref _listenOnConditionPool.Value.Get(recipientEntityUnpack);
                if(!listenOnConditionComp.ConditionSenders.Contains(notifyOnConditionComp.SenderCondition))
                    listenOnConditionComp.ConditionSenders.Add(notifyOnConditionComp.SenderCondition);
                _notifyOnConditionPool.Value.Del(entity);
            }
        }
    }
}