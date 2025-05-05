using Client;

using Statement;
using Leopotam.EcsLite;

namespace AbilitySystem
{
    [System.Serializable]
    public class KnockbackEffect : StatusEffect
    {
        private bool isGetUp;

        public KnockbackEffect(KnockbackEffect data)
        {
            _status_id = "knockback";
            _lifeTime = data._lifeTime;
        }

        public override void AddEffect(EcsPackedEntity owner, EcsPackedEntity sender)
        {
             _ownerEntity = owner;
             _senderEntity = sender;

             var world = BattleState.Instance.EcsRunHandler.World;

             if (owner.Unpack(world, out int entity))
             {
                 if (world.GetPool<KnockbackComponent>().Has(entity)) return;

                 world.GetPool<KnockbackComponent>().Add(entity);

                 //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.KnockBack, entity);
             }
        }

        public override IEffect Clone()
        {
            return new KnockbackEffect(this);
        }

        public override void RemoveEffect()
        {
            var world = BattleState.Instance.EcsRunHandler.World;

            if (_ownerEntity.Unpack(world, out int entity))
            {
                if (world.GetPool<KnockbackComponent>().Has(entity)) world.GetPool<KnockbackComponent>().Del(entity);

                world.GetPool<EffectsContainer>().Get(entity).RemoveEffect(this);
            }
        }

        public override void Run()
        {
            base.Run();

            if(_lifeTime <= 0.75f && !isGetUp) GetUp();
        }

        public override void Update(IEffect e)
        {
            var stunEffect = e as KnockbackEffect;

            _lifeTime = stunEffect.GetLifeTime();
        }

        void GetUp()
        {
            var world = BattleState.Instance.EcsRunHandler.World;

            if (_ownerEntity.Unpack(world, out int entity))
                if (!world.GetPool<KnockbackAnimationState>().Has(entity))
                    world.GetPool<KnockbackAnimationState>().Add(entity).KnockbackState = KnockbackState.getup;

            isGetUp = true;
        }
    }
}