using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterCounterPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterCounter, PerkComponent>, Exc<UnusedPerk>> _filter = default;
        readonly EcsPoolInject<AfterCounter> _afterCounterPool = default;
        readonly EcsPoolInject<UnusedPerk> _unusedPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterCounterPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var counterComp = ref _afterCounterPool.Value.Get(entity);
                counterComp.CurrentCount = Mathf.Clamp(counterComp.CurrentCount + 1, 0, counterComp.TargetCount);
                if(counterComp.CurrentCount < counterComp.TargetCount)
                {
                    _unusedPool.Value.Add(entity);
                }
                else
                {
                    counterComp.CurrentCount = 0;
                }
            }
        }
    }
}