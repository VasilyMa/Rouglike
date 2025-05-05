using Leopotam.EcsLite;
namespace Client {
    struct AddPerkRequest {
        public Perk Perk;
        public EcsPackedEntity TargetEntity;
    }
}