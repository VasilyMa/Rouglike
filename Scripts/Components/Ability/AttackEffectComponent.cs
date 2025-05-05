using System.Collections.Generic;

using AbilitySystem;

using Leopotam.EcsLite;

namespace Client 
{
    struct AttackEffectComponent : IAbilityComponent
    {
        public int AttackCount;
        public int TargetAttackCount;

        public List<EffectAmplifier> Effects;

        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {

        }

        public void Init()
        {

        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            AttackCount++;

            if (AttackCount >= TargetAttackCount)
            {
                ResolveEffect();
                AttackCount = 0;
            }
        }

        public void AddEffect(EffectAmplifier effect)
        {
            var existEffect = Effects.Find(x => effect.EffectName == x.EffectName);

            if (existEffect != null)
            {
                existEffect.IncreaseEffect();
                return;
            }

            Effects.Add(effect.Clone());
        }

        public void ResolveEffect()
        {
            foreach (var effect in Effects)
            {
                effect.InvokeEffect();
            }
        }
    }
}