using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;

namespace Client
{
    sealed class DecisionMakingSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain/*,RequestingActionTag*/>, Exc<InActionComponent, HardControlComponent>> _filter = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<IdleRequest> _idleRequestPool = default;
        readonly EcsPoolInject<MoveToTargetRequest> _moveToTargetRequestPool = default;
        readonly EcsPoolInject<AttackRequest> _attackRequestPool = default;
        readonly EcsPoolInject<DefendRequest> _defendRequestPool = default;
        readonly EcsPoolInject<SupportRequest> _supportRequestPool = default;
        readonly EcsPoolInject<KeepAtRangeRequest> _keepAtRangeRequestPool = default;
        readonly EcsPoolInject<TerrorizeRequest> _terrorizeRequestPool = default;
        readonly EcsPoolInject<WanderingRequest> _wanderingRequestPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new DecisionMakingSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                AIState keyOfMaxValue = unitBrain.statesScore.OrderByDescending(entry => entry.Value).FirstOrDefault().Key;

                // for debug purposes only -->
                unitBrain.PriorityStateScore = unitBrain.statesScore[keyOfMaxValue];
                // <--
                

                switch (keyOfMaxValue)
                {

                    case AIState.Idle:
                        TryAddRequestToEntity(_idleRequestPool.Value, entity);
                        break;
                    case AIState.MoveTo:
                        TryAddRequestToEntity(_moveToTargetRequestPool.Value, entity);
                        break;
                    case AIState.Attack:
                        TryAddRequestToEntity(_attackRequestPool.Value, entity);
                        break;
                    case AIState.Defend:
                        TryAddRequestToEntity(_defendRequestPool.Value, entity);
                        break;
                    case AIState.Support:
                        TryAddRequestToEntity(_supportRequestPool.Value, entity);
                        break;
                    case AIState.KeepAtRange:
                        TryAddRequestToEntity(_keepAtRangeRequestPool.Value, entity);
                        break;
                    case AIState.Terrorize:
                        TryAddRequestToEntity(_terrorizeRequestPool.Value, entity);
                        break;
                    case AIState.Wandering:
                        TryAddRequestToEntity(_wanderingRequestPool.Value, entity);
                        break;
                }
                unitBrain.CurrentState = keyOfMaxValue;
            }
        }

        private void TryAddRequestToEntity<T>(EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity)) pool.Add(entity);
        }
    }
}