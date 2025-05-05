using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class AddPerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddPerkRequest>> _filter = default;
        readonly EcsPoolInject<AddPerkRequest> _requestPool = default;
        readonly EcsPoolInject<PerkComponent> _perkPool = default;
        readonly EcsPoolInject<PerkResolveBlockComponent> _perkResolveBlockPool = default;
        public override MainEcsSystem Clone()
        {
            return new AddPerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var requestComp = ref _requestPool.Value.Get(entity);
                EcsWorld world = BattleState.Instance.EcsRunHandler.World;

                int perkEntity = world.NewEntity();
                foreach(var comp in requestComp.Perk.Conditions)
                {
                    comp.InitPerkComponent(perkEntity, world);
                }
                ref var resolveBlockComp = ref _perkResolveBlockPool.Value.Add(perkEntity);

                resolveBlockComp.ResolveEffectsOriginal = new System.Collections.Generic.List<IPerkResolveEffect>(requestComp.Perk.Resolve);
                resolveBlockComp.ResolveEffectsModified = new System.Collections.Generic.List<IPerkResolveEffect>(requestComp.Perk.Resolve);
                resolveBlockComp.DisposeEffectsOriginal = new System.Collections.Generic.List<IDisposeResolvePerkCondition>(requestComp.Perk.DisposeConditions);
                resolveBlockComp.DisposeEffectsModified = new System.Collections.Generic.List<IDisposeResolvePerkCondition>(requestComp.Perk.DisposeConditions);

                PlayerEntity.Instance.PerkCollectionData.AddTemporaryPerk(new CurrentPerkData(requestComp.Perk.KEY_ID, 1));

                _perkPool.Value.Add(perkEntity);
                _requestPool.Value.Del(entity);
            }
        }
    }
}