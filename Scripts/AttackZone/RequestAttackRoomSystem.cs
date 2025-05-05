using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Statement;

namespace Client {
    sealed class RequestAttackRoomSystem : MainEcsSystem {
        
        readonly EcsFilterInject<Inc<RequestAttackRoomEvent>> _filter;
        readonly EcsPoolInject<RequestAttackRoomEvent> _requestAttackRoomPool;
        readonly EcsFilterInject <Inc<AttackRoomComponent>> _filterAttack;
        readonly EcsPoolInject<AttackRoomComponent> _attackRoomPool;
        readonly EcsPoolInject<InitAttackRoomEvent> _initAttackRoomPool;

        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new RequestAttackRoomSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entityRequest in _filter.Value)
            {
                ref var requestAttackRoom = ref _requestAttackRoomPool.Value.Get(entityRequest);
                foreach(var entityAttack in _filterAttack.Value)
                {
                    ref var attackRoomPool = ref _attackRoomPool.Value.Get(entityAttack);
                    if (attackRoomPool.attackZone.IndexAttackZone == requestAttackRoom.IndexAttack) attackRoomPool.attackZone.Dispose();
                }
                if (!BattleState.Instance.CurrentRoom.CheckIndexAttackZone(requestAttackRoom.IndexAttack, entityRequest, _world.Value)) continue;
                ref var initAttackRoom = ref _initAttackRoomPool.Value.Add(entityRequest);
                initAttackRoom.TimeToResolve = requestAttackRoom.TimeToResolve;
                initAttackRoom.LifeTime = requestAttackRoom.LifeTime;
                initAttackRoom.Components = new(requestAttackRoom.Components);
                initAttackRoom.Delay = requestAttackRoom.Delay;
            }
        }
    }
}