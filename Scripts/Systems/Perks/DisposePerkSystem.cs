using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client
{
    public class DisposePerkSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<HelperPerk>, Exc<UnusedHelper>> _filter = default;
        readonly EcsPoolInject<HelperPerk> _helperPool = default;
        readonly EcsPoolInject<BusyPerk> _busyPool = default;

        //this invoked before pre init and init
        public override MainEcsSystem Clone()
        {
            return new DisposePerkSystem();
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var helperEntity in _filter.Value)
            {
                ref var helperComp = ref _helperPool.Value.Get(helperEntity);
                foreach(var effect in helperComp.ResolveEffects)
                {
                    effect.DisposePerk(helperEntity, BattleState.Instance.EcsRunHandler.World);
                }
                if(helperComp.OwnerPerkEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int perkEntity))
                {
                    _busyPool.Value.Del(perkEntity);
                }
                BattleState.Instance.EcsRunHandler.World.DelEntity(helperEntity);
            }
        }
    }
}