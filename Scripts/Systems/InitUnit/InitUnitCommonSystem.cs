using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Client
{
    /// <summary>
    /// Adding common components that are present on both the player and the enemies
    /// </summary>
    sealed class InitUnitCommonSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ViewComponent, InitUnitEvent>> _filter;
        readonly EcsPoolInject<ViewComponent> _viewPool;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<ColliderComponent> _colliderPool = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<RigidbodyComponent> _rigidbodyPool = default;
        readonly EcsPoolInject<MeshComponent> _meshPool = default;
        readonly EcsPoolInject<AnimatorComponent> _animatorPool = default;
        readonly EcsPoolInject<EffectsContainer> _effectsContainerPool = default;
        readonly EcsPoolInject<UnitComponent> _unitPool = default;
        readonly EcsPoolInject<VisualEffectsComponent> _visualEffectPool = default;
        readonly EcsPoolInject<AbilityEffectsContainer> _abilityEffectsContainerPool = default;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new InitUnitCommonSystem();
        }

        readonly EcsPoolInject<ResistanceConditionComponent> _resistanceConditionsPool;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                var GO = viewComp.GameObject;
                //transform
                ref var transformComp = ref _transformPool.Value.Add(entity);
                transformComp.Transform = GO.transform;
                //navMesh
                ref var navMeshComp = ref _navMeshPool.Value.Add(entity);
                navMeshComp.NavMeshAgent = GO.GetComponent<NavMeshAgent>();
                navMeshComp.NavMeshAgent.enabled = true;
                navMeshComp.NavMeshAgent.ResetPath();
                navMeshComp.NavMeshAgent.isStopped = true;
                //Collider
                ref var colliderComp = ref _colliderPool.Value.Add(entity);
                colliderComp.Collider = GO.GetComponent<Collider>();
                colliderComp.RagDollCollider = GO.GetComponentsInChildren<Collider>();
                //Animator
                ref var animatorComp = ref _animatorPool.Value.Add(entity);
                animatorComp.Animator = GO.GetComponent<Animator>();
                //RigingBody
                ref var rigidbodyComp = ref _rigidbodyPool.Value.Add(entity);
                rigidbodyComp.Rigidbody = GO.GetComponent<Rigidbody>();
                rigidbodyComp.RagDollRigidBody = GO.GetComponentsInChildren<Rigidbody>();
                rigidbodyComp.Rigidbody.velocity = Vector3.zero;
                rigidbodyComp.Rigidbody.angularVelocity = Vector3.zero;
                //AttackZone
                ref var attackComp = ref _attackPool.Value.Add(entity);
                attackComp.AttackZoneReference = GO.GetComponentInChildren<AttackZone>();
                attackComp.AttackZoneReference.Init(_world.Value, entity);
                
                //mesh
                ref var meshComp = ref _meshPool.Value.Add(entity);
                meshComp.SkinnedMeshRenderers = GO.GetComponentsInChildren<SkinnedMeshRenderer>();
                //unitComp
                ref var unitComp = ref _unitPool.Value.Add(entity);
                //effect
                ref var effectContainer = ref _effectsContainerPool.Value.Add(entity);
                effectContainer.Init();
                ref var visualComp = ref _visualEffectPool.Value.Add(entity);
                visualComp.Init();
                if(GO.TryGetComponent<DashParticle>(out DashParticle particle))
                {
                    visualComp.DashParticle = particle;
                }
                //resistCondition
                ref var resistanceConditionsComp = ref _resistanceConditionsPool.Value.Add(entity);
                resistanceConditionsComp.resistConditions = new();//poka pusto potom sdelat dobavlenie
                //condition
                ref var conditionContainerComponent = ref _conditionContainerPool.Value.Add(entity);
                conditionContainerComponent.Conditions = new();
            }
        }
    }
}