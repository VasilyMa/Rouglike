using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    /// <summary>
    /// We add components that are not present on the enemies
    /// </summary>
    sealed class SpecialPlayerInit : MainEcsSystem {
    readonly EcsFilterInject<Inc<InitUnitEvent, PlayerComponent>> _filter;
    readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool;
    readonly EcsSharedInject<GameState> _state;
    readonly EcsPoolInject<HealthComponent> _healthPool = default;
    readonly EcsPoolInject<StaminaComponent> _staminaPool = default;

    public override MainEcsSystem Clone()
    {
        return new SpecialPlayerInit();
    }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var unitMBComp = ref _unitMBPool.Value.Get(entity);
                var gameConfig = ConfigModule.GetConfig<GameConfig>();
                //weapon
                var weaponConfig = ConfigModule.GetConfig<AllWeaponConfig>().GetWeaponByID(PlayerEntity.Instance.Weapons.CurrentWeaponData.KEY_ID);
                unitMBComp.AbilityUnitMB.WeaponConfig = weaponConfig;
                //health

                ref var healthComp = ref _healthPool.Value.Add(entity);

                var playerData = PlayerEntity.Instance.PlayerData;
                
                if (playerData.Health > 0)
                {
                    healthComp.Init(playerData.Health, gameConfig.MaxHealthPlayer);
                }
                else
                {
                    healthComp.Init(gameConfig.MaxHealthPlayer, gameConfig.MaxHealthPlayer);
                }
                //stamina
                ref var staminaComp = ref _staminaPool.Value.Add(entity);
                staminaComp.Init(100);

                var relicConfig = ConfigModule.GetConfig<RelicConfig>();

                if (PlayerEntity.Instance.RelicCollectionData.CurrentRelicData.Count > 0)
                {
                    var currentRelicData = PlayerEntity.Instance.RelicCollectionData.CurrentRelicData;

                    foreach (var relicTmpData in currentRelicData)
                    {
                        if (relicConfig.TryGetRelic(relicTmpData.KEY_ID, out var relic))
                        {
                            relic.LoadRelic();
                        }
                    }
                }
            }
        }
    }
}
