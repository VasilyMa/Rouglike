using Leopotam.EcsLite;
namespace AbilitySystem
{
    public interface IAbilityBaseComponent
    {
        void Init(int entity, EcsWorld world);
    }
}
