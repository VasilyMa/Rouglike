using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class ResolvePerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<PerkComponent, PerkResolveBlockComponent>, Exc<UnusedPerk, BusyPerk>> _filter = default;
        readonly EcsPoolInject<PerkResolveBlockComponent> _resolveBlockPool = default;
        readonly EcsPoolInject<HelperPerk> _helperPerkPool = default;
        readonly EcsPoolInject<BusyPerk> _busyPerkPool = default;
        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new ResolvePerkSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //remove this if it not used
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                var world = BattleState.Instance.EcsRunHandler.World;
                _busyPerkPool.Value.Add(entity);
                ref var resolveBlockComp = ref _resolveBlockPool.Value.Get(entity);
                //создать сущность, которая проконтролирует резолв, на нее повесить все компоненты из резолв блока перка
                int helperEntity = world.NewEntity();

                ref var helperComp = ref _helperPerkPool.Value.Add(helperEntity);
                helperComp.ResolveEffects = new(resolveBlockComp.ResolveEffectsModified);
                helperComp.OwnerPerkEntity = world.PackEntity(entity);
                
                foreach(var effect in resolveBlockComp.ResolveEffectsModified)
                {
                    effect.ResolvePerk(helperEntity, world);
                }
                foreach(var disposeCondition in resolveBlockComp.DisposeEffectsModified)
                {
                    disposeCondition.InitCondition(helperEntity, world);
                }
            }
        }
    }
}