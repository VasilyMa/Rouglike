using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class AIRotationSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, AnimatorComponent>, Exc<LockRotationComponent,SpawnAbilityEvent,StaticUnitComponent>> _filter = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<AnimatorComponent> _animatorPool = default;

        public override MainEcsSystem Clone()
        {
            return new AIRotationSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var unitBrain = ref _brainPool.Value.Get(unitEntity);
                ref var transformComp = ref _transformPool.Value.Get(unitEntity);
                transformComp.Transform.LookAt(unitBrain.priorityPointToLook);
                ref var animatorComp = ref _animatorPool.Value.Get(unitEntity);
                Vector3 direction = unitBrain.priorityPointToMove - transformComp.Transform.position;
                Vector3 inversedDirection = transformComp.Transform.InverseTransformDirection(direction).normalized;
                animatorComp.Animator.SetFloat("yAxis", inversedDirection.z, 0.1f, Time.deltaTime);
                animatorComp.Animator.SetFloat("xAxis", inversedDirection.x, 0.1f, Time.deltaTime);
            }
        }
    }
}