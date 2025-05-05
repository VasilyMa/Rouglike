using Leopotam.EcsLite;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    struct SpawnStaticUnitEvent 
    {
        public Vector3 position;
        public GameObject GameObject;
        public EcsPackedEntity OwnnerEntity;
        public float Health;
        public bool isImmortal;
        public AIProfile AIprofile;
        public List<AbilityBase> abilities;
        public bool delDesynchronization;
        public Transform Parent;
    }
}