using UnityEngine;

namespace Client {
    struct DropEvent {
        public Vector3 DropPosition;
        public Vector3 EndPosition;
        public IDrop dropItem;
    }
}