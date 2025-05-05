using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;
using Statement;

namespace Client {
    sealed class InitRelicSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<RelicPoolComponent> _relicPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitRelicSystem();
        }

        public override void Init (IEcsSystems systems) 
        {
            var relicPoolEntity = _world.Value.NewEntity();

            State.Instance.RegisterNewEntity("RelicPoolEntity", relicPoolEntity);

            ref var relicPoolComp = ref _relicPool.Value.Add(relicPoolEntity);

            var relicConfig = ConfigModule.GetConfig<RelicConfig>();

            var relicData = PlayerEntity.Instance.RelicCollectionData;

            var tempList = new List<RelicResource>();

            foreach (var relic in relicData.Relics)
            {
                if (!relic.IsLocked)
                {
                    tempList.Add(relicConfig.GetRelicByID(relic.KEY_ID));
                }
            }
            relicPoolComp.RelicPool = tempList;
            relicPoolComp.ChanceDropRare = relicConfig.RareDrop;
            relicPoolComp.ChanceDropCommon = relicConfig.CommonDrop;
        }
    }
}