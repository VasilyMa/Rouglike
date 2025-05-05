using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;
using System.Collections.Generic;

namespace Client
{
    public class InitPlayersPerksSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PlayerComponent, InitUnitEvent>> _filter = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AddPerkRequest> _addPerkPool = default;


        readonly EcsPoolInject<AddPerkRequest> _requestPool = default;
        readonly EcsPoolInject<PerkComponent> _perkPool = default;
        readonly EcsPoolInject<PerkResolveBlockComponent> _perkResolveBlockPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new InitPlayersPerksSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var playerEntity in _filter.Value)
            {
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(playerEntity);

                var perkList = new List<Perk>();

                var perkConfig = ConfigModule.GetConfig<PerkConfig>();

                var perkPlayerData = PlayerEntity.Instance.PerkCollectionData.CurrentPerkData;

                foreach (var perkData in perkPlayerData)
                {
                    var perk = perkConfig.GetPerkByID(perkData.KEY_ID);

                    if (perk != null) LoadPerk(perk);
                }
            }
        }

        void LoadPerk(Perk perk)
        {
            EcsWorld world = BattleState.Instance.EcsRunHandler.World;

            int perkEntity = world.NewEntity();

            foreach (var comp in perk.Conditions)
            {
                comp.InitPerkComponent(perkEntity, world);
            }

            ref var resolveBlockComp = ref _perkResolveBlockPool.Value.Add(perkEntity);

            resolveBlockComp.ResolveEffectsOriginal = new System.Collections.Generic.List<IPerkResolveEffect>(perk.Resolve);
            resolveBlockComp.ResolveEffectsModified = new System.Collections.Generic.List<IPerkResolveEffect>(perk.Resolve);
            resolveBlockComp.DisposeEffectsOriginal = new System.Collections.Generic.List<IDisposeResolvePerkCondition>(perk.DisposeConditions);
            resolveBlockComp.DisposeEffectsModified = new System.Collections.Generic.List<IDisposeResolvePerkCondition>(perk.DisposeConditions);

            _perkPool.Value.Add(perkEntity);
        }
    }
}