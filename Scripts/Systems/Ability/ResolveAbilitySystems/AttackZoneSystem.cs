using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class InvokeAttackZoneSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<InvokeAttackZoneEvent>> _filter = default;
        readonly EcsPoolInject<InvokeAttackZoneEvent> _invokePool = default;
        readonly EcsPoolInject<AttackZoneComponent> _attackZonePool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<DisableAttackZoneComponent> _disableAttackZonePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<RecalculateResolveBlockEvent> _recalculatePool = default;

        public override MainEcsSystem Clone()
        {
            return new InvokeAttackZoneSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var invokeComp = ref _invokePool.Value.Get(entity);
                if(invokeComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    int attackZoneEntity = _world.Value.NewEntity();

                    ref var attackZoneComp = ref _attackZonePool.Value.Add(attackZoneEntity);
                    attackZoneComp.OwnerEntity = invokeComp.OwnerEntity;
                    attackZoneComp.AbilityEntity = invokeComp.AbilityEntity;
                    ref var attackComp = ref _attackPool.Value.Get(ownerEntity);
                    ref var transformComp = ref _transformPool.Value.Get(ownerEntity);

                    attackZoneComp.AttackZone = PoolModule.Instance.GetFromPool<AttackZone>(attackComp.AttackZoneReference, true);
                    attackZoneComp.AttackZone.transform.SetParent(transformComp.Transform);
                    attackZoneComp.AttackZone.transform.localPosition = new Vector3(0, 1, 0);
                    attackZoneComp.AttackZone.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    attackZoneComp.AttackZone.transform.localScale = invokeComp.Size;


                    attackZoneComp.AttackZone.Init(_world.Value, attackZoneEntity);
                    attackZoneComp.AttackZone.SetAttackZoneMesh(invokeComp.AttackZoneMesh);
                    attackZoneComp.AttackZone.AttackZoneEnable();

                    ref var attackTransform = ref _transformPool.Value.Add(attackZoneEntity);
                    attackTransform.Transform = attackZoneComp.AttackZone.transform;
                    
                    ref var recalculateComp = ref _recalculatePool.Value.Add(attackZoneEntity);
                    recalculateComp.AbilityEntity = invokeComp.AbilityEntity;

                    ref var disableComp = ref _disableAttackZonePool.Value.Add(attackZoneEntity);
                    disableComp.DisableTime = invokeComp.DisableTime;
                }
            }
        }
    }
}