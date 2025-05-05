using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using UnityEngine.AI;
using UI;
using Cinemachine;
using FMODUnity;
using Statement;

namespace Client
{
    sealed class InitPlayerSystem : MainEcsSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<InitUnitEvent> _initUnitPool = default;
        readonly EcsPoolInject<InitModifiersEvent> _initModifiersPool = default;


        public override MainEcsSystem Clone()
        {
            return new InitPlayerSystem();
        }

        public override void Init(IEcsSystems systems)
        {
            var entity = _world.Value.NewEntity();

            State.Instance.RegisterNewEntity("PlayerEntity", entity);

            _playerPool.Value.Add(entity);
            _initUnitPool.Value.Add(entity);
            ref var viewComp = ref _viewPool.Value.Add(entity);

            var gameConfig = ConfigModule.GetConfig<GameConfig>();
            viewComp.GameObject = GameObject.Instantiate(gameConfig.PlayerGO);

            var weaponConfig = ConfigModule.GetConfig<AllWeaponConfig>();
            var abilitiesConfig = ConfigModule.GetConfig<AllAbilityConfig>();

            var playerWeaponData = PlayerEntity.Instance.Weapons.CurrentWeaponData;

            PlayerEntity.Instance.Currency.FavourChange(0);
            PlayerEntity.Instance.Currency.EffigiesChange(0);

            var weapon = weaponConfig.GetWeaponByID(playerWeaponData.KEY_ID);


            viewComp.GameObject.GetComponent<AbilityUnitMB>().WeaponConfig = weapon;


            var cinemachineCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
            cinemachineCamera.Follow = viewComp.GameObject.transform;
            cinemachineCamera.LookAt = viewComp.GameObject.transform;

            StudioListener studioListener = Camera.main.GetComponent<StudioListener>();
            studioListener.attenuationObject = viewComp.GameObject;

            _initModifiersPool.Value.Add(entity);
        }
    }
}