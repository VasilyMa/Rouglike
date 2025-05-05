using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class InvokeMissileSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InvokeMissileEvent>> _filter = default;
        readonly EcsPoolInject<InvokeMissileEvent> _invokePool = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<RecalculateResolveBlockEvent> _recalculatePool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;
        readonly EcsPoolInject<MissileManagerComponent> _missileManagerComponent;

        public override MainEcsSystem Clone()
        {
            return new InvokeMissileSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value) // entity - unit
            {
                ref var invokeComp = ref _invokePool.Value.Get(entity);

                if (invokeComp.AbilityPackedEntity.Unpack(_world.Value, out int abilityEntity))
                {
                    if(invokeComp.OwnerPackedEntity.Unpack(_world.Value, out int OwnerEntity))
                    {
                        int missileEntity = _world.Value.NewEntity();
                        ref var recalculateComp = ref _recalculatePool.Value.Add(missileEntity);
                        recalculateComp.AbilityEntity = invokeComp.AbilityPackedEntity;
                        ref var missleComp = ref _missilePool.Value.Add(missileEntity);
                        missleComp.missile = invokeComp.missile;
                        missleComp.Offset = invokeComp.Offset;
                        missleComp.Speed = invokeComp.Speed;
                        missleComp.Invoke(missileEntity, OwnerEntity, abilityEntity, _world.Value);
                        //todo resolve block clone
                        missleComp.missile.Invoke(_world.Value, missileEntity, LayerMask.NameToLayer(missleComp.LayerNameTarget));
                        float charge = 1;
                        if (_chargePool.Value.Has(abilityEntity))
                        {
                            ref var chargeComp = ref _chargePool.Value.Get(abilityEntity);
                            charge = chargeComp.CurrentCharge;
                        }
                        //foreach (var comp in invokeComp.Components)
                        //{
                        //    comp.Invoke(missileEntity, OwnerEntity, abilityEntity, _world.Value, charge);
                        //}
                        ref var missileManager = ref _missileManagerComponent.Value.Add(missileEntity);
                        missileManager.AbilityPackedEntity = invokeComp.AbilityPackedEntity;
                        missileManager.OwnerPackedEntity = invokeComp.OwnerPackedEntity;
                        missileManager.Components = new(invokeComp.Components);
                        missileManager.currentInedexComponent = 0 ;
                        missileManager.charge = charge;
                        missileManager.Components[0].Invoke(missileEntity,_world.Value, charge);
                        
                    }

                }
            }
        }
    }
}