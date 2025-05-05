using UnityEngine;

namespace Client {
    struct MoveComponent {
        public Vector3 TargetPosition;
        public Vector3 MoveDirection;
        public bool IsAroundTarget;
    }
}