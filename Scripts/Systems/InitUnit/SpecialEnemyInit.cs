using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Statement;

namespace Client {
    /// <summary>
    /// Adding something that is not on the player
    /// </summary>
    sealed class SpecialEnemyInit : MainEcsSystem {
        readonly EcsFilterInject<Inc<InitUnitEvent, EnemyComponent>> _filter;
        readonly EcsPoolInject<TelegraphingUnitComponent> _telegraphingUnitPool;
        readonly EcsPoolInject<PhysicsUnitComponent> _physiclUnitPool;
        readonly EcsPoolInject<CreateEnemyEvent> _createEnemyEvent;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;
        readonly EcsPoolInject<HighToughnessComponent> _highToughnessPool;
        readonly EcsPoolInject<ViewComponent> _viewPool;
        readonly EcsPoolInject<InitAIEvent> _initAIPool = default;
        readonly EcsPoolInject<InitContextEvent> _initContextPool = default;

        public override MainEcsSystem Clone()
        {
            return new SpecialEnemyInit();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var createEnemyComp = ref _createEnemyEvent.Value.Get(entity);
                var enemyConfig = createEnemyComp.UnitConfig;
                //Telegraphing
                var viewConfig = ConfigModule.GetConfig<ViewConfig>();
                ref var viewComp = ref _viewPool.Value.Get(entity);
                var GO = viewComp.GameObject;
                ref var telegraphingUnitComp = ref _telegraphingUnitPool.Value.Add(entity);
                telegraphingUnitComp.TelegraphingUnitMB = GO.GetComponent<TelegraphingUnitMB>();
                telegraphingUnitComp.TelegraphingUnitMB.TeleGO = viewConfig.TeleGO;
                telegraphingUnitComp.TelegraphingUnitMB.MegaTeleGO = viewConfig.MegaTeleGO;
                telegraphingUnitComp.TelegraphingUnitMB.DeathParticle = GO.GetComponentInChildren<ParticleSystem>();
                telegraphingUnitComp.TelegraphingUnitMB.Init(entity);
                //health
                ref var healthComp = ref _healthPool.Value.Add(entity);
                healthComp.Init(enemyConfig.Health, enemyConfig.Health); 
                //unitComp
                ref var unitComp = ref _unitPool.Value.Get(entity);
                unitComp.MaxSpeed = enemyConfig.Speed;
                //NavMesh
                ref var navMeshComp = ref _navMeshPool.Value.Get(entity);
                navMeshComp.NavMeshAgent.speed = enemyConfig.Speed;
                //toughness
                if (createEnemyComp.UnitConfig.MaxValueToughness > 0)
                {
                    ref var toughnessComp = ref _toughnessPool.Value.Add(entity);
                    toughnessComp.MaxValueToughness = createEnemyComp.UnitConfig.MaxValueToughness;
                    toughnessComp.CurrentValue = toughnessComp.MaxValueToughness;
                    toughnessComp.DelayRecoveryToughness = createEnemyComp.UnitConfig.DelayRecoveryToughness;
                    toughnessComp.SpeedRecovery = createEnemyComp.UnitConfig.GetSpeedRecovery();
                    if (toughnessComp.MaxValueToughness > 0) _highToughnessPool.Value.Add(entity);
                }
               
                //AI

                ref var initAIComp = ref _initAIPool.Value.Add(entity);
                initAIComp.AIprofile = createEnemyComp.UnitConfig.AIProfile;
                _initContextPool.Value.Add(entity);
            }
        }
    }
}