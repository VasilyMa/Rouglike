using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.VisualScripting;
using UnityEngine;

namespace Client
{
    public class CheckAddHitIntervalSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<HitAnimationState,/*PlayerComponent,*/AnimatorComponent>, Exc<DeadComponent, SpawnAnimationState, HardHitComponent,AddHitIntervalEvent>> _filter = default;
        readonly EcsPoolInject<AddHitIntervalEvent> _addHitIntervalPool;
        readonly EcsPoolInject<HitAnimationState> _hitAnimatorStatePool;
        readonly EcsWorldInject _world;
        public override MainEcsSystem Clone()
        {
            return new CheckAddHitIntervalSystem();
        }

        public override void Init(IEcsSystems systems)
        { 
        }

        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var addHitIntervalComp = ref _addHitIntervalPool.Value.Add(_world.Value.NewEntity());
                ref var hitAnimationState = ref _hitAnimatorStatePool.Value.Get(entity);
                addHitIntervalComp.TargetEntity = _world.Value.PackEntity(entity);
                addHitIntervalComp.Type = hitAnimationState.Type;
            }

        }
    }
}