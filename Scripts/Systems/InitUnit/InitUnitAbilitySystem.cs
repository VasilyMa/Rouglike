using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;
using Statement;

namespace Client {
/// <summary>
/// We throw abilities into the ability component
/// </summary>
    sealed class InitUnitAbilitySystem : MainEcsSystem {
        readonly EcsSharedInject<GameState> _gameState;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ViewComponent, InitUnitEvent, AbilityUnitComponent>, Exc<PlayerComponent>> _nonPlayerFilter = default;
        readonly EcsFilterInject<Inc<ViewComponent, InitUnitEvent, AbilityUnitComponent, PlayerComponent>> _playerFilter = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitUnitAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _playerFilter.Value)
            {
                var state = State.Instance;
                var abilityData = PlayerEntity.Instance.AbilityCollectionData;
                ref var unitMBComp = ref _unitMBPool.Value.Get(entity);
                EcsPackedEntity ownerEntity = _world.Value.PackEntity(entity);
                // weaponComp should be inited anyway for correct mesh initialization
                ref var weaponComp = ref _weaponPool.Value.Add(entity);
                weaponComp.Init(unitMBComp.AbilityUnitMB);
                if (!unitMBComp.AbilityUnitMB.TemporaryAnimatorOverrideController)
                {
                    unitMBComp.AbilityUnitMB.TemporaryAnimatorOverrideController = Object.Instantiate(unitMBComp.AbilityUnitMB.WeaponConfig.AnimatorOverrideController);
                    unitMBComp.AbilityUnitMB.TemporaryAnimatorOverrideController.name = "TemporaryAnimatorOverrideController";
                    unitMBComp.AbilityUnitMB.Animator.runtimeAnimatorController = unitMBComp.AbilityUnitMB.TemporaryAnimatorOverrideController;
                }
                // if abilityData already exists we should init abilities from this data instead of initing it from weapon
                if (abilityData.CurrentAbilitiesData.Count > 0)
                {
                    var allAbilityConfig = ConfigModule.GetConfig<AllAbilityConfig>();

                    foreach (var abilityInfo in abilityData.CurrentAbilitiesData)
                    {
                        if (allAbilityConfig.TryGetAbilityByID(abilityInfo.KEY_ID, out AbilityBase loadedAbility))
                        {
                            ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                            initAbilityComp.AbilityBase = loadedAbility;
                            initAbilityComp.PackedEntity = ownerEntity;
                            initAbilityComp.initFromCollection = true;
                            initAbilityComp.NewAbilityInputReference = abilityInfo.ReferenceInput;
                        }
                    }
                }
                else
                {
                    foreach (var ability in weaponComp.Abilities)
                    {
                        AbilityBase setAbility = ability;
                        ref var inputComp = ref _inputPool.Value.Get(state.GetEntity("InputEntity"));
                        if (ability.SourceAbility.InputActionReference.action.name == inputComp.InputAction.ActionMap.SuperAttack.name)
                        {
                            var allAbility = ConfigModule.GetConfig<AllAbilityConfig>();
                            setAbility = allAbility.GetAbilityByID(PlayerEntity.Instance.Weapons.CurrentWeaponData.CurrentSecondaryAbilityID);
                        }
                        ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                        initAbilityComp.AbilityBase = setAbility;
                        initAbilityComp.PackedEntity = ownerEntity;
                    }
                    foreach (var ability in unitMBComp.AbilityUnitMB.NonAttackAbilities)
                    {
                        ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                        initAbilityComp.AbilityBase = ability;
                        initAbilityComp.PackedEntity = ownerEntity;
                    }
                }
            }
            foreach(var entity in _nonPlayerFilter.Value)
            {
                var state = State.Instance;

                ref var unitMBComp = ref _unitMBPool.Value.Get(entity);
                EcsPackedEntity ownerEntity = _world.Value.PackEntity(entity);
                ref var weaponComp = ref _weaponPool.Value.Add(entity);
                weaponComp.Init(unitMBComp.AbilityUnitMB);
                foreach (var ability in weaponComp.Abilities)
                {
                    AbilityBase setAbility = ability;
                    ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                    initAbilityComp.AbilityBase = setAbility;
                    initAbilityComp.PackedEntity = ownerEntity;
                }
                foreach (var ability in unitMBComp.AbilityUnitMB.NonAttackAbilities)
                {
                    ref var initAbilityComp = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                    initAbilityComp.AbilityBase = ability;
                    initAbilityComp.PackedEntity = ownerEntity;
                }
            }
        }
    }
}