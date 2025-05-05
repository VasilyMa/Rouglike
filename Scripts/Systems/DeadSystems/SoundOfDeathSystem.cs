using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SoundOfDeathSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<SoundUnitComponent, MomentDeadEvent>> _filter;
        readonly EcsPoolInject<SoundUnitComponent> _soundUnitPool;

        public override MainEcsSystem Clone()
        {
            return new SoundOfDeathSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var soundUnitComp = ref _soundUnitPool.Value.Get(entity);
                soundUnitComp.SoundUnitMB.DeathSound();
            }
        }
    }
}