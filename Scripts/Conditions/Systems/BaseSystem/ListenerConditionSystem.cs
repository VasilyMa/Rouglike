using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;

namespace Client {
    sealed class ListenerConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<ListenerConditionComponent, ListenOnConditionEvent>> _filter;
        readonly EcsPoolInject<ListenerConditionComponent> _listenerConditionPool;
        readonly EcsPoolInject<ListenOnConditionEvent> _listenOnConditionPool;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;

        public override MainEcsSystem Clone()
        {
            return new ListenerConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var listenOnConditionComp = ref _listenOnConditionPool.Value.Get(entity);
                ref var listenerConditionComp = ref _listenerConditionPool.Value.Get(entity);
                foreach(var conditionSender in listenOnConditionComp.ConditionSenders)
                {
                    var listenerConditionData = listenerConditionComp.listListeners.FirstOrDefault(x => x.ConditionSender == conditionSender);
                    if (listenerConditionData is null) continue;
                    if (!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                    ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                    resolveConditionComp.resolve.AddRange(listenerConditionData.Resolve);
                }
            }
        }
    }
}