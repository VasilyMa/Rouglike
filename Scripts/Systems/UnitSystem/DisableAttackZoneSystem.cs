using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisableAttackZoneSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DisableAttackZoneComponent, AttackZoneComponent>, Exc<DelEntityEvent>> _filter = default;
        readonly EcsPoolInject<DisableAttackZoneComponent> _disablePool = default;
        readonly EcsPoolInject<AttackZoneComponent> _attackPool = default;
        readonly EcsPoolInject<DelEntityEvent> _delEvt = default;

        public override MainEcsSystem Clone()
        {
            return new DisableAttackZoneSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var disableComp = ref _disablePool.Value.Get(entity);
                disableComp.Timer += Time.deltaTime;
                if(disableComp.Timer >= disableComp.DisableTime)
                {
                    ref var attackComp = ref _attackPool.Value.Get(entity);
                    attackComp.AttackZone.AttackZoneDisable();
                    _disablePool.Value.Del(entity);
                    _attackPool.Value.Del(entity);

                    _delEvt.Value.Add(entity);
                }
            }
        }
    }
}