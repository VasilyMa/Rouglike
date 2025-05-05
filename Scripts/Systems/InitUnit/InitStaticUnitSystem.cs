using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitStaticUnitSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<SpawnStaticUnitEvent>> _filter;
        readonly EcsPoolInject<SpawnStaticUnitEvent> _spawnStaticObjectPool;
        readonly EcsPoolInject<AttackComponent> _attackPool;
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<StaticUnitComponent> _staticUnitPool;
        readonly EcsPoolInject<HealthComponent> _healthComponent;
        readonly EcsPoolInject<VisualEffectsComponent> _visualEffectPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitComponent;
        readonly EcsPoolInject<ViewComponent> _viewPool;
        readonly EcsPoolInject<DelDesynchronizationComponentcs> _delDesynchonizationPool;
        readonly EcsPoolInject<EnemyComponent> _enemyPool;
        readonly EcsPoolInject<ChildUnitsComponent> _childUnitsComponent;

        public override MainEcsSystem Clone()
        {
            return new InitStaticUnitSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var spawnComponent = ref _spawnStaticObjectPool.Value.Get(entity);
                var GO = GameObject.Instantiate(spawnComponent.GameObject, spawnComponent.position, Quaternion.identity);
                if (spawnComponent.Parent != null)
                {
                    GO.transform.parent = spawnComponent.Parent;
                    GO.transform.position = spawnComponent.Parent.transform.position;
                    GO.transform.rotation = spawnComponent.Parent.transform.rotation;
                }
                var abilityUnitComp = GO.GetComponent<AbilityUnitMB>();
                _abilityUnitComponent.Value.Add(entity).AbilityUnitMB = abilityUnitComp;
                abilityUnitComp._entity = entity;

                ref var attackComp = ref _attackPool.Value.Add(entity);
                attackComp.AttackZoneReference = GO.GetComponentInChildren<AttackZone>();
                attackComp.AttackZoneReference.Init(_world.Value, entity);

                ref var staticUnitComponent = ref _staticUnitPool.Value.Add(entity);
                staticUnitComponent.ownerEntity = spawnComponent.OwnnerEntity;

                ref var transformComponent = ref _transformPool.Value.Add(entity);
                transformComponent.Transform = GO.transform;

                _viewPool.Value.Add(entity);
                _enemyPool.Value.Add(entity);
                if (spawnComponent.delDesynchronization) _delDesynchonizationPool.Value.Add(entity);
                ref var visualComp = ref _visualEffectPool.Value.Add(entity);
                visualComp.Init();

                if (!spawnComponent.isImmortal)
                {
                    ref var healthComponent = ref _healthComponent.Value.Add(entity);
                    healthComponent.Init(spawnComponent.Health, spawnComponent.Health);
                }
                foreach (var ability in spawnComponent.abilities)
                {
                    var abilityEntity = _world.Value.NewEntity();
                    ref var InitAbilityEvent = ref _initAbilityPool.Value.Add(abilityEntity);
                    InitAbilityEvent.PackedEntity = _world.Value.PackEntity(entity);
                    InitAbilityEvent.AbilityBase = ability;
                }

                if(spawnComponent.OwnnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    if (!_childUnitsComponent.Value.Has(ownerEntity)) _childUnitsComponent.Value.Add(ownerEntity).childUnits = new();
                    ref var childComp = ref _childUnitsComponent.Value.Get(ownerEntity);
                    childComp.childUnits.Add(_world.Value.PackEntity(entity));
                }
                ref var animComp = ref _world.Value.GetPool<AnimatorComponent>().Add(entity);
                animComp.Animator = GO.GetComponent<Animator>();
            }
        }
    }
}