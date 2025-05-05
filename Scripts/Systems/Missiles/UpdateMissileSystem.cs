using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpdateMissileSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MissileComponent, TargetMissileComponent>> _filterTarget;
        readonly EcsFilterInject<Inc<MissileComponent, CasterMissileComponent>> _filterCaster;
        readonly EcsPoolInject<MissileComponent> _missilePool;
        readonly EcsPoolInject<TargetMissileComponent>  _targetMissilePool;
        readonly EcsPoolInject<CasterMissileComponent>  _casterMissileComponent;
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<TransformComponent> _transformPool;

        public override MainEcsSystem Clone()
        {
            return new UpdateMissileSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filterTarget.Value)
            {
                ref var missileComponent = ref _missilePool.Value.Get(entity);
                ref var targetMissileComp = ref _targetMissilePool.Value.Get(entity);
                if (targetMissileComp.EntityTarget.Unpack(_world.Value, out int targetEntity))
                {
                    if (_transformPool.Value.Has(targetEntity))
                    {
                        ref var transformComp = ref _transformPool.Value.Get(targetEntity);
                        missileComponent.TargetPosition = transformComp.Transform.position;
                    }
                    else
                        _targetMissilePool.Value.Del(entity);
                }
                else
                    _targetMissilePool.Value.Del(entity);
            }
            foreach(var entity in _filterCaster.Value)
            {
                ref var missileComponent = ref _missilePool.Value.Get(entity);
                ref var casterMissileCompoent = ref _casterMissileComponent.Value.Get(entity);
                if (casterMissileCompoent.EntityCaster.Unpack(_world.Value, out int entityCaster))
                {
                    if (_transformPool.Value.Has(entityCaster))
                    {
                        ref var transformComp = ref _transformPool.Value.Get(entityCaster);
                        missileComponent.CasterPosition = transformComp.Transform.position;
                    }
                    else
                        _casterMissileComponent.Value.Del(entity);
                }
                else
                    _casterMissileComponent.Value.Del(entity);
            }
        }
    }
}