
using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client {
    sealed class EnemyLogicsSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TargetComponent>, Exc<DeadComponent>> _filter = default;
        readonly EcsPoolInject<TargetComponent> _targetPool = default;
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<EnemyComponent> _enemyPool = default;
        
        readonly EcsPoolInject<AttackButtonDownEvent> _attackButtonDownPool = default;
        readonly EcsPoolInject<SpecAttackButtonDownEvent> _specAttackButtonDownPool = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var targetComp = ref _targetPool.Value.Get(entity);

                var world = State.Instance.EcsRunHandler.World;

                if(targetComp.TargetPackedEntity.Unpack(world, out int targetEntity))
                {
                    ref var enemyComp = ref _enemyPool.Value.Get(entity);
                    ref var transformComp = ref _transfromPool.Value.Get(entity);
                    ref var targetViewComp = ref _transfromPool.Value.Get(targetEntity);

                    if(enemyComp.EnemyType == EnemyTypes.Melee)
                    {
                        var distance = Vector3.Distance(transformComp.Transform.position, targetViewComp.Transform.position);

                        if(distance <= enemyComp.AgroDistance)
                        {
                            //todo стукаем ?
                            _attackButtonDownPool.Value.Add(entity);
                            
                            transformComp.Transform.LookAt(targetViewComp.Transform);
                            transformComp.Transform.rotation = Quaternion.Euler(0, transformComp.Transform.rotation.eulerAngles.y, 0);
                        }
                        else if(distance > enemyComp.AgroDistance && distance <= 10)
                        {
                            //todo прыгаем

                            _specAttackButtonDownPool.Value.Add(entity);
                            
                            transformComp.Transform.LookAt(targetViewComp.Transform);
                            transformComp.Transform.rotation = Quaternion.Euler(0, transformComp.Transform.rotation.eulerAngles.y, 0);
                        }
                    }
                    else
                    {
                        var distance = Vector3.Distance(transformComp.Transform.position, targetViewComp.Transform.position);
                        if(distance <= enemyComp.AvoidDistance)
                        {
                            _specAttackButtonDownPool.Value.Add(entity);

                            transformComp.Transform.LookAt(targetViewComp.Transform);
                            transformComp.Transform.rotation = Quaternion.Euler(0, transformComp.Transform.rotation.eulerAngles.y, 0);
                        }
                        else if(distance > enemyComp.AvoidDistance && distance <= enemyComp.AgroDistance)
                        {
                            _attackButtonDownPool.Value.Add(entity);

                            transformComp.Transform.LookAt(targetViewComp.Transform);
                            transformComp.Transform.rotation = Quaternion.Euler(0, transformComp.Transform.rotation.eulerAngles.y, 0);
                        }
                    }
                }
            }
        }
    }
}