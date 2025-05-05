using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class AfterAbilityTagPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AfterAbilityTag, PerkComponent>, Exc<UnusedPerk>> _perkFilter = default;
        readonly EcsFilterInject<Inc<AbilityMainPhaseResolveEvent>> _abilityMainPhaseFilter = default;
        readonly EcsPoolInject<AfterAbilityTag> _afterAbilityPool = default;
        readonly EcsPoolInject<AbilityMainPhaseResolveEvent> _mainResolvePool = default;
        readonly EcsPoolInject<UnusedPerk> _unusedPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new AfterAbilityTagPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var perkEntity in _perkFilter.Value)
            {
                bool serviceCheck = false;
                ref var afterAbilityComp = ref _afterAbilityPool.Value.Get(perkEntity);
                foreach(var resolveEntity in _abilityMainPhaseFilter.Value)
                {
                    ref var mainResolveComp = ref _mainResolvePool.Value.Get(resolveEntity);
                    if((afterAbilityComp.AbilityTargetTag & mainResolveComp.AbilityTags) == afterAbilityComp.AbilityTargetTag)
                    {
                        serviceCheck = true;
                    }
                }
                if(!serviceCheck) _unusedPool.Value.Add(perkEntity);
                
            }
        }
    }
}