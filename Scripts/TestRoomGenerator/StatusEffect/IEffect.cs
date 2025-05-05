using Leopotam.EcsLite;

namespace AbilitySystem
{
    public interface IEffect
    {
        IEffect Clone();
        void Run();
        void Update(IEffect effect);
        void AddEffect(EcsPackedEntity owner, EcsPackedEntity sender);
        void RemoveEffect();
        string GetId();
        float GetLifeTime();
    }
}