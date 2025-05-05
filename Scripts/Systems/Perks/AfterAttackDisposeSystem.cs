using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using AbilitySystem;
using UnityEngine;

namespace Client
{
    public class AfterAbilityTagDisposeSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterAbilityTagDispose>, Exc<UnusedHelper>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityMainPhaseResolveEvent>> _abilityMainPhaseFilter = default;
        readonly EcsPoolInject<UnusedHelper> _unusedHelperPool = default;
        readonly EcsPoolInject<AfterAbilityTagDispose> _afterAbilityPool = default;
        readonly EcsPoolInject<AbilityMainPhaseResolveEvent> _mainResolvePool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterAbilityTagDisposeSystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var helperEntity in _filter.Value)
            {
                bool serviceCheck = false;
                ref var afterAbilityComp = ref _afterAbilityPool.Value.Get(helperEntity);
                foreach(var resolveEntity in _abilityMainPhaseFilter.Value)
                {
                    ref var mainResolveComp = ref _mainResolvePool.Value.Get(resolveEntity);
                    if((afterAbilityComp.AbilityTargetTag & mainResolveComp.AbilityTags) == afterAbilityComp.AbilityTargetTag)
                    {
                        serviceCheck = true;
                    }
                }
                if(!serviceCheck) _unusedHelperPool.Value.Add(helperEntity);
                
            }
        }
    }
}