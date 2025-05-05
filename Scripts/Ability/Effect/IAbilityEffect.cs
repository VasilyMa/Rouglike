
using Leopotam.EcsLite;

namespace AbilitySystem
{
    public interface IAbilityEffect
    {
        void Invoke(int entity, int entitySender,EcsWorld world);
        void Recalculate(float charge);
    }
}
