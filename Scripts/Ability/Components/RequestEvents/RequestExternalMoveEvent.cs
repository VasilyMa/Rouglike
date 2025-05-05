using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Client {
    struct RequestExternalMoveEvent : IAbilityComponent
    {
        [HideInInspector] public Vector3 MoveDirection;
        public DirectionType Direction;
        [HideIf("Direction", DirectionType.ToTarget)] public ForceMode ForceMode;
        [HideIf("Direction", DirectionType.ToTarget)] public float Speed;
        [ShowIf("Direction", DirectionType.ToTarget)] public float ExtraDistance;
        public float Duration;
        [HideIf("Direction", DirectionType.ToTarget)] public bool IsInterruptible;
        [HideInInspector] public EcsPackedEntity OwnerPackedEntity;
        [HideInInspector] public EcsPackedEntity AbilityPackedEntity;
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        { }

        public void Init()
        { }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestExternalMoveEvent>().Add(world.NewEntity());
            requestComp.OwnerPackedEntity = world.PackEntity(ownerEntity);
            requestComp.AbilityPackedEntity = world.PackEntity(abilityEntity);
            requestComp.Speed = Speed;
            requestComp.Duration = Duration;
            requestComp.ForceMode = ForceMode;
            requestComp.Direction = Direction;
            requestComp.MoveDirection = MoveDirection;
            requestComp.IsInterruptible = IsInterruptible;
        }
        public enum DirectionType
        {Forward, WASD, ToTarget }
    }
}