using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckBossStageSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<BossComponent, HealthComponent>,Exc<DeadComponent,InActionComponent,HardControlComponent, ChangeBossStageEvent>> _filter;
        readonly EcsPoolInject<HealthComponent> _healthPool;
        readonly EcsPoolInject<BossComponent> _bossPool;
        readonly EcsPoolInject<ChangeBossStageEvent> _changeBossStagePool;

        public override MainEcsSystem Clone()
        {
            return new CheckBossStageSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var bossEntity in _filter.Value)
            {
                ref var bossComp = ref _bossPool.Value.Get(bossEntity);
                ref var healthComp = ref _healthPool.Value.Get(bossEntity);

                int newStageIndex = GetStageIndex(ref healthComp, ref bossComp);

                if (newStageIndex != bossComp.CurrentStage)
                {
                    if (newStageIndex < 0 || newStageIndex > bossComp.StageCount)
                        return;

                    bossComp.CurrentStage = newStageIndex;
                    //send event change abilities
                    _changeBossStagePool.Value.Add(bossEntity);
                }
            }
        }

        int GetStageIndex(ref HealthComponent healthComponent, ref BossComponent bossComponent)
        {
            if (bossComponent.StageCount == 0)
                return -1;

            float healthPerStage = healthComponent.MaxValue / bossComponent.StageCount;

            for (int i = 0; i < bossComponent.StageCount; i++)
            {
                if (healthComponent.CurrentValue > healthComponent.MaxValue - healthPerStage * (i + 1))
                {
                    return i+1; 
                }
            }

            return -1;
        }

    }
}