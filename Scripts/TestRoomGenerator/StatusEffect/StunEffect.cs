using Client;
using UnityEngine;
using Leopotam.EcsLite;
using Statement;

namespace AbilitySystem
{
    [System.Serializable]
    public class StunEffect : StatusEffect
    {
        [SerializeField] private SourceParticle _stunEffect;

        public StunEffect(StunEffect data)
        {
            _status_id = "stun";

            if (data._stunEffect is not null)
            {
                _lifeTime = data._lifeTime;
                _attachEffect = data._attachEffect;
                _stunEffect = GameObject.Instantiate(data._stunEffect);
                _stunEffect.gameObject.SetActive(false);
            }
        }

        public override void Update(IEffect e)
        {
            var stunEffect = e as StunEffect;

            _lifeTime = stunEffect.GetLifeTime();
        }

        public override IEffect Clone()
        {
            return new StunEffect(this);
        }

        public override void AddEffect(EcsPackedEntity owner, EcsPackedEntity sender)
        {
            _ownerEntity = owner;
            _senderEntity = sender;

            var world = BattleState.Instance.EcsRunHandler.World;

            if (owner.Unpack(world, out int entity))
            {
                world.GetPool<StunComponent>().Add(entity);

                ref var transformCoponent = ref world.GetPool<TransformComponent>().Get(entity);
                
                //to do stun animation
                //ChangeAnimationController.ChangeAnimationFunc(AnimationTypes.Idle, entity);

                if (_stunEffect is null) return;
            }
        }

        public override void RemoveEffect()
        {
            var world = BattleState.Instance.EcsRunHandler.World;

            if (_ownerEntity.Unpack(world, out int entity))
            {
                if (_stunEffect is not null) _stunEffect.Dispose();

                world.GetPool<StunComponent>().Del(entity);
                world.GetPool<TelegraphingUnitComponent>().Get(entity).TelegraphingUnitMB.DeactiveTeleGO();

                world.GetPool<EffectsContainer>().Get(entity).RemoveEffect(this);
            }
        }
    }
}