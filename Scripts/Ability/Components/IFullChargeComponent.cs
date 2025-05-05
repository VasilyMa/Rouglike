using Leopotam.EcsLite;
namespace AbilitySystem
{
    public interface IFullChargeComponent
    {
        public void Invoke(int ownerEntity, EcsWorld world);
    }
}
