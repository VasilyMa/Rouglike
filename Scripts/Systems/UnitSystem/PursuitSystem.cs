//using Leopotam.EcsLite;
//using UnityEngine;
//using Leopotam.EcsLite.Di;

//namespace Client
//{
//    sealed class PursuitSystem : MainEcsSystem
//    {
//        readonly EcsFilterInject<Inc<SlowPursuit>, Exc<DeadComponent>> _filter = default;
//        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
//        readonly EcsPoolInject<UnitComponent> _unitPool = default;
//        readonly EcsPoolInject<LockMoveComponent> _lockMovePool = default;
//        readonly EcsPoolInject<LockRotationComponent> _lockRotationPool = default;

//        public override MainEcsSystem Clone()
//        {
//            return new PursuitSystem();
//        }

//        public override void Run(IEcsSystems systems)
//        {
//            foreach (var entity in _filter.Value)
//            {
//                ref var viewComp = ref _navMeshPool.Value.Get(entity);
//                ref var unitComp = ref _unitPool.Value.Get(entity);
//                //viewComp.NavMeshAgent.speed = unitComp.SlowSpeed;
//                _lockMovePool.Value.Del(entity);
//                _lockRotationPool.Value.Del(entity);
//            }
//        }
//    }
//}