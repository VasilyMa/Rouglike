using Leopotam.EcsLite;
namespace AbilitySystem {
    public interface IAbilityMissileComponent 
    {
        void Invoke(int entity,EcsWorld world, float charge);
    }
}