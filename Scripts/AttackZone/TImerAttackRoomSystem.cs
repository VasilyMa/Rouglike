using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class TimerAttackRoomSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<TimerAttackRoomComponent,AttackRoomComponent>> _filter;
        readonly EcsPoolInject<TimerAttackRoomComponent> _timerAttackAttackPool;
        readonly EcsPoolInject<AttackRoomComponent> _atackRoomPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new TimerAttackRoomSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var timerAttackComp = ref _timerAttackAttackPool.Value.Get(entity);
                ref var attackRoomComp = ref _atackRoomPool.Value.Get(entity);
                timerAttackComp.LifeTime -= Time.deltaTime;
                if(timerAttackComp.LifeTime <= 0 )
                {
                    attackRoomComp.attackZone.Dispose();
                    continue;
                }
                timerAttackComp.Delay -= Time.deltaTime;
                if (timerAttackComp.Delay >= 0) continue;
                timerAttackComp.TimerForResolve -= Time.deltaTime;
                if (timerAttackComp.TimerForResolve > 0) continue;
                attackRoomComp.attackZone.AllCollisionUnit();
                timerAttackComp.TimerForResolve = timerAttackComp.TimeToResolve;
            }
        }
    }
}