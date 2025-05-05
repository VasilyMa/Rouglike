using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CirclingSystem : MainEcsSystem
    {        
        readonly EcsFilterInject<Inc<EnemyComponent, TargetComponent, Circling>, Exc<LockMoveComponent, LockRotationComponent, MoveComponent>> _filter = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<Circling> _circlingPool = default;
        readonly EcsPoolInject<NavMeshComponent> _navmeshPool = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<LockRotationComponent> _lockRotationPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationComp = default;

        public override MainEcsSystem Clone()
        {
            return new CirclingSystem();
        }

        public override void Run(IEcsSystems systems)
        {
        }
        /* public void Run (IEcsSystems systems) {
    foreach (int entity in _filter.Value)
    {
        ref var abilityComp = ref _abilityUnitPool.Value.Get(entity);
        var entities = abilityComp.AbilityUnitMB.GetAllAbilitiesEntities();
        bool hasSomethingToCast = false;
        foreach (int abilityEntity in entities)
        {
            if (!_recalculationComp.Value.Has(abilityEntity))
            {
                hasSomethingToCast = true;
            }
        }
        if (hasSomethingToCast)
        {
            ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Move, entity);
            _circlingPool.Value.Del(entity);
            continue;
        }
        ref var AIComp = ref _AIPool.Value.Get(entity);
        UpdateCircularMovement(entity);
        UpdateDirectionChange(entity);
        ChangeAnimation(entity, AIComp.currentAngle, AIComp.unitBehaviourType);
    }
}

private void UpdateCircularMovement(int entity)
{
    ref var AIComp = ref _AIPool.Value.Get(entity);
    ref var targetViewComp = ref _transformPool.Value.Get(GameState.Instance.PlayerEntity);
    ref var moveComp = ref _movePool.Value.Add(entity);
    Vector3 newPosition = targetViewComp.Transform.position + Quaternion.Euler(0f, AIComp.currentAngle, 0f) * (Vector3.forward * AIComp.WalkingRadius);
    moveComp.TargetPosition = newPosition;
    if (!_lockRotationPool.Value.Has(entity))
    {
        ref var transformComp = ref _transformPool.Value.Get(entity);
        transformComp.Transform.LookAt(targetViewComp.Transform.position);
    }
}

private void UpdateDirectionChange(int entity)
{
    ref var AIComp = ref _AIPool.Value.Get(entity);
    if (Time.time - AIComp.lastDirectionChange > AIComp.changeDirectionDelay)
    {
        AIComp.angleVelocity = Random.Range(-90f, 90f);
        AIComp.lastDirectionChange = Time.time;
    }
    AIComp.currentAngle += Time.deltaTime * AIComp.angleVelocity;

}

private void ChangeAnimation(int entity, float currentAngle, UnitConfig.UnitBehaviourType unitType)
{
    AnimationTypes type;
    float spiderAnimSpeedForMirror = 1;
    if (currentAngle >= -90f && currentAngle < 90f)
    {
        type = AnimationTypes.RunRight;
    }
    else
    {
        type = AnimationTypes.RunLeft;
    }
    if (unitType == UnitConfig.UnitBehaviourType.Melee)
    {
        ChangeAnimationController.ChangeAnimationFunc(type, entity);
    }
    else if (unitType == UnitConfig.UnitBehaviourType.Spider)
    {
        ChangeAnimationController.ChangeAnimationFunc(type, entity, animationSpeed: spiderAnimSpeedForMirror);
    }
    else if (unitType == UnitConfig.UnitBehaviourType.Bibich)
        ChangeAnimationController.ChangeAnimationFunc(type, entity, animationSpeed: 0.5f);
}*/
    }
}