using Leopotam.EcsLite;
namespace Client {
    struct TakeDamageComponent {
        public float Damage;
        public EcsPackedEntity KillerEntity;
        public EcsPackedEntity TargetEntity;
    }
   
}