// using Leopotam.EcsLite;
// using Leopotam.EcsLite.Di;
// namespace Client {
//     sealed class CheckRecoverySystem : MainEcsSystem
//     {
//         readonly EcsFilterInject<Inc<TakeDamageComponent, ToughnessComponent>, Exc<HighToughnessComponent, RecoveryEvent, DeadComponent>> _damageEventFilter = default;
//         readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;
//         readonly EcsPoolInject<RecoveryEvent> _recoveryEventPool = default;

//         public override MainEcsSystem Clone()
//         {
//             return new CheckRecoverySystem();
//         }

//         public override void Run (IEcsSystems systems) {
//             foreach (int entity in _damageEventFilter.Value)
//             {
//                 ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
//                 if (toughnessComp.CurrentValue <= 0)
//                 {
//                     _recoveryEventPool.Value.Add(entity);
//                 }
//             }
//         }
//     }
// }