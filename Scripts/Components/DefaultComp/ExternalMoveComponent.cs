using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    struct ExternalMoveComponent
    {
        public Vector3 MoveDirection;
        public RequestExternalMoveEvent.DirectionType Direction;
        public ForceMode ForceMode;
        public float Speed;
        public float Duration;
        public Vector3 SupportDirection;
        public bool IsInterruptible;

        public void Invoke(Vector3 moveDirection, ForceMode force, float speed, float duration = 1, Vector3 supportDirection = new Vector3(), bool isInterruptible = true, RequestExternalMoveEvent.DirectionType directionType = RequestExternalMoveEvent.DirectionType.Forward)
        {
            ForceMode = force;
            MoveDirection = moveDirection;
            Duration = duration;
            Speed = speed;
            SupportDirection = supportDirection;
            IsInterruptible = isInterruptible;
            Direction = directionType;
        }
    }
}