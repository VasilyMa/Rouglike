using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ChangeAbilityBossStageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<BossComponent, HealthComponent, ChangeBossStageEvent>, Exc<DeadComponent, InActionComponent, HardControlComponent>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityComponent,OwnerComponent>> _abilityFilter = default;
        readonly EcsPoolInject<ChangeBossStageEvent> _pool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<BossComponent> _bossPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool = default;

        readonly EcsPoolInject<AttacksContext> _attacksContext = default;
        readonly EcsPoolInject<TerrorizeContext> _terrorizeContext = default;
        readonly EcsPoolInject<DefenseContext> _defenseContext = default;
        readonly EcsPoolInject<SupportContext> _supportContext = default;
        readonly EcsPoolInject<InitContextEvent> _initContext = default;

        public override MainEcsSystem Clone()
        {
            return new ChangeAbilityBossStageSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var bossEntity in _filter.Value)
            {
                ref var bossComp = ref _bossPool.Value.Get(bossEntity);

                foreach (var abilityEntity in _abilityFilter.Value)
                {
                    ref var ownerComp = ref _ownerPool.Value.Get(abilityEntity);
                    if (ownerComp.OwnerEntity.Unpack(_world.Value, out int entityOwner))
                    {
                        if (entityOwner == bossEntity)
                        {
                            _world.Value.DelEntity(abilityEntity);
                        }
                    }
                }

                /* if (_attacksContext.Value.Has(bossEntity))
                 {
                     ref var attacksContext = ref _attacksContext.Value.Get(bossEntity);
                     attacksContext.attackAbilitiesList = new List<EcsPackedEntity>();
                     attacksContext.validAbilitiesList = new List<EcsPackedEntity>();
                 }
                 if (_terrorizeContext.Value.Has(bossEntity))
                 {
                     ref var terrorContext = ref _terrorizeContext.Value.Get(bossEntity);
                     terrorContext.terrorizeAbilitiesList = new List<EcsPackedEntity>();
                     terrorContext.validAbilitiesList = new List<EcsPackedEntity>();
                 }
                 if (_defenseContext.Value.Has(bossEntity))
                 {
                     ref var defenseContext = ref _defenseContext.Value.Get(bossEntity);
                     defenseContext.defenseActionsList = new List<EcsPackedEntity>();
                 }
                 if (_supportContext.Value.Has(bossEntity))
                 {
                     ref var suppContext = ref _supportContext.Value.Get(bossEntity);
                     suppContext.supportAbilitiesList = new List<EcsPackedEntity>();
                 }*/
                foreach (var ability in bossComp.BossStages[bossComp.CurrentStage-1].Abilities)
                {
                    ref var initAbility = ref _initAbilityPool.Value.Add(_world.Value.NewEntity());
                    initAbility.PackedEntity = _world.Value.PackEntity(bossEntity);
                    initAbility.AbilityBase = ability;
                }
                _initContext.Value.Add(bossEntity);
                _pool.Value.Del(bossEntity);
            }
        }
    }
}