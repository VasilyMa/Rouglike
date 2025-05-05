using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterCounterDisposeSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterCounterDispose>, Exc<UnusedHelper>> _filter = default;
        readonly EcsPoolInject<AfterCounterDispose> _counterPool = default;
        readonly EcsPoolInject<UnusedHelper> _unusedHelperPool = default;
        public override MainEcsSystem Clone()
        {
            return new AfterCounterDisposeSystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var helperEntity in _filter.Value)
            {
                ref var counterComp = ref _counterPool.Value.Get(helperEntity);
                counterComp.CurrentCount++;
                if(counterComp.CurrentCount < counterComp.TargetCount)
                {
                    _unusedHelperPool.Value.Add(helperEntity);
                }
            }
        }
    }
}