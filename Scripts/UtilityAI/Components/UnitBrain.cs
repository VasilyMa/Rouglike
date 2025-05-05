using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    struct UnitBrain {
        public float PriorityStateScore;
        public Dictionary<AIState, float> statesScore;
        public AIState CurrentState;
        public EcsPackedEntity bestAttackAvailable;
        public EcsPackedEntity bestSupportActionAvailable;
        public EcsPackedEntity bestDefensiveActionAvailable;
        public EcsPackedEntity bestTerrorizeActionAvailable;
        public Vector3 priorityPointToMove;
        public Vector3 priorityPointToLook;
        public bool IsTriggeredUnit;
        public float ReactionToAction;
    }
}