using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct NotifyOnConditionEvent : IConditionResolve
    {
        public Condition RecipientCondition;
        [HideInInspector] public Condition SenderCondition;
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
        {
            var _conditionPool = world.GetPool<ConditionCompnent>();
            if (!_conditionPool.Has(entityCondition)) return;
            ref var conditionComp = ref _conditionPool.Get(entityCondition);
            ref var notifyOnConditionEvent = ref world.GetPool<NotifyOnConditionEvent>().Add(world.NewEntity());
            notifyOnConditionEvent.OwnerEntity = world.PackEntity(entityOwner);
            notifyOnConditionEvent.SenderCondition = conditionComp.Condition;
            notifyOnConditionEvent.RecipientCondition = RecipientCondition;
        }

        public void Recalculate(float charge)
        {
        }
    }
}