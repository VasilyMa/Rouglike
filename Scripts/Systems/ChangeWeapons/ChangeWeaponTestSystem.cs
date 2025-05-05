using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Client
{
    sealed class ChangeWeaponTestSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ChangeWeaponEvent>, Exc<HardControlComponent, InActionComponent>> _filter = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<ChangeWeaponEvent> _changeWeaponPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool = default;
        readonly EcsWorldInject _world = default;
        List<WeaponConfig> configs;
        AllWeaponConfig weaponConfig;

        public override MainEcsSystem Clone()
        {
            return new ChangeWeaponTestSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            //configs = LoadConfigs();
            weaponConfig = ConfigModule.GetConfig<AllWeaponConfig>();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);
                ref var changeWeaponEvent = ref _changeWeaponPool.Value.Get(entity);

                foreach (var abilityEntity in abilityUnitComp.AbilityUnitMB.GetAllAbilitiesEntities())
                {
                    _world.Value.DelEntity(abilityEntity);
                }

                abilityUnitComp.AbilityUnitMB.AllAbilities = new Dictionary<string, List<EcsPackedEntity>>();
                PlayerEntity.Instance.AbilityCollectionData.Reset();
                var weapon = weaponConfig.GetWeaponByID(changeWeaponEvent.weapon_ID);
                abilityUnitComp.AbilityUnitMB.WeaponConfig = weapon;
                ref var weaponComp = ref _weaponPool.Value.Get(entity);
                weaponComp.Init(abilityUnitComp.AbilityUnitMB);
                foreach (var ability in weaponComp.Abilities)
                {
                    ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                    initAbilityComp.AbilityBase = ability;
                    initAbilityComp.PackedEntity = _world.Value.PackEntity(entity);
                }
                foreach (var ability in abilityUnitComp.AbilityUnitMB.NonAttackAbilities)
                {
                    ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                    initAbilityComp.AbilityBase = ability;
                    initAbilityComp.PackedEntity = _world.Value.PackEntity(entity);
                }
                _changeWeaponPool.Value.Del(entity);
            }
        }

        /*public List<WeaponConfig> LoadConfigs()
        {
            List<WeaponConfig> configs = new List<WeaponConfig>();
            string configsFolderPath = "Assets/Resources/Configs/WeaponConfigs";

            if (Directory.Exists(configsFolderPath))
            {
                string[] files = Directory.GetFiles(configsFolderPath, "*.asset");
                foreach (string file in files)
                {
                    string assetPath = "Assets" + file.Substring(Application.dataPath.Length);
                    WeaponConfig[] configAssets = Resources.LoadAll<WeaponConfig>("Configs/WeaponConfigs");
                    foreach (WeaponConfig config in configAssets)
                    {
                        configs.Add(config);
                    }
                }
            }
            return configs;
        }*/
    }
}
