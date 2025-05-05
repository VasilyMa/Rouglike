using AbilitySystem;

namespace Client 
{
    struct ShieldCock 
    {
        public float AbsorbDamage;

        public void Init(IEffect effect)
        {
        }

        public float ReduceDamage(float damage)
        {
            float differenceValue = AbsorbDamage - damage;

            AbsorbDamage -= damage;

            if (differenceValue > 0) return 1;

            return differenceValue;
        }

        public void Remove()
        {
        }
    }
}