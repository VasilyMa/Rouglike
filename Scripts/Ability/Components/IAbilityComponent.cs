using Leopotam.EcsLite;
namespace AbilitySystem
{
    public interface IAbilityComponent
    {
        void Init();
        void Invoke(int ownerEntity,int abilityEntity, EcsWorld world, float charge = 1);
        void Dispose(int entityCaster, int abilityEntity, EcsWorld world);
    }
}
