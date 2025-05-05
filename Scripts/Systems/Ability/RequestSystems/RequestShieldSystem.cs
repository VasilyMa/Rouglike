using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;
using Statement;

namespace Client {
    sealed class RequestShieldSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestShieldEvent>> _filter;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<ShieldsContainer> _shieldContainerPool;
        readonly EcsPoolInject<RequestShieldEvent> _requestShieldPool;
        readonly EcsPoolInject<NonWaitClickable> _nonWaitPool;

        public override MainEcsSystem Clone()
        {
            return new RequestShieldSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                //��������� � ���������
                ref var requestShield = ref _requestShieldPool.Value.Get(entity);
                requestShield.TargetPackedEntity.Unpack(_world.Value, out int targetEntity);
                if (!_shieldContainerPool.Value.Has(targetEntity))
                {
                    ref var shieldContainerComponent = ref _shieldContainerPool.Value.Add(targetEntity);
                    shieldContainerComponent.shieldComponents = new();
                }
                ref var shieldContainerComp = ref _shieldContainerPool.Value.Get(targetEntity);
                var newShield = new Shield( requestShield.SourceParticle, requestShield.DamageProtection, requestShield.Duration);
                int index = shieldContainerComp.shieldComponents.BinarySearch(newShield, Comparer<Shield>.Create((x, y) => x.Duration.CompareTo(y.Duration)));
                if (index < 0) index = ~index; // ���������� ���������� ���������, ����� �������� ������
               shieldContainerComp.shieldComponents.Insert(index, newShield);

                //������� �� ���
                ref var transformComp = ref State.Instance.EcsRunHandler.World.GetPool<TransformComponent>().Get(targetEntity);
                newShield.Invoke(transformComp.Transform);
                if (!_nonWaitPool.Value.Has(targetEntity)) _nonWaitPool.Value.Add(targetEntity);
            }
        }
    }
}