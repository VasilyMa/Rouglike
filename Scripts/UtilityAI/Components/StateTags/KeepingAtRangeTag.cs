using UnityEngine;

namespace Client {
    struct KeepingAtRangeTag {
        public Transform transformToKeepAtRange;
        public float distanceToKeep;
        public float timeLeftToDirectionSwitch;
        public int directionModifier;
    }
}