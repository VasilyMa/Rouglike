using JetBrains.Annotations;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct ListenerConditionComponent : IConditionComponent
    {
        [SerializeReference] public List<ListenerConditionData> listListeners;
        public void Invoke(int entityCondition, EcsWorld world)
        {
            var _listerConditionPool = world.GetPool<ListenerConditionComponent>();
            if (!_listerConditionPool.Has(entityCondition)) _listerConditionPool.Add(entityCondition).listListeners = new();
            ref var listenerConditionComp = ref _listerConditionPool.Get(entityCondition);
            listenerConditionComp.listListeners.AddRange(listListeners);
        }
    }
}
[Serializable]
class ListenerConditionData
{
    public Condition ConditionSender;
    [SerializeReference] public List<IConditionResolve> Resolve;
}