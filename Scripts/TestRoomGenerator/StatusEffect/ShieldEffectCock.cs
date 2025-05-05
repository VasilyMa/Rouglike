using Client;

using Leopotam.EcsLite;
using UnityEngine;
using Statement;

namespace AbilitySystem
{
    [System.Serializable]
    public class ShieldEffectCock : StatusEffect
    {
        [SerializeField] private SourceParticle _shieldEffect;
        [SerializeField] private float _durableShield;
        private GameObject shieldEffectGO = null;

        public ShieldEffectCock(ShieldEffectCock data)
        {
            _status_id = "shield";

            if (data is not null)
            {
                _attachEffect = data._attachEffect;
                _shieldEffect = data._shieldEffect;
                _lifeTime = data._lifeTime;
                _durableShield = data._durableShield;
            }
        }

        public override IEffect Clone()
        {
            return new ShieldEffectCock(this);
        }

        public override void AddEffect(EcsPackedEntity owner, EcsPackedEntity sender)
        {
            _ownerEntity = owner;
            _senderEntity = sender;

            var world = BattleState.Instance.EcsRunHandler.World;

            if (owner.Unpack(world, out int entity))
            {
                if (world.GetPool<ShieldCock>().Has(entity)) return;

                ref var shieldComp = ref world.GetPool<ShieldCock>().Add(entity);
                shieldComp.AbsorbDamage = _durableShield;
                shieldComp.Init(this);

                ref var unitMBComponent = ref world.GetPool<TelegraphingUnitComponent>().Get(entity);
                Transform attachTarget = unitMBComponent.TelegraphingUnitMB.GetMemberOfBodyByType(_attachEffect);

                if (_shieldEffect is null) return;
                shieldEffectGO = GameObject.Instantiate(_shieldEffect).gameObject;
                
                shieldEffectGO.transform.SetParent(attachTarget);
                shieldEffectGO.transform.localPosition = Vector3.zero;
            };
        }

        public override void RemoveEffect()
        {
            var world = BattleState.Instance.EcsRunHandler.World;

            if (_ownerEntity.Unpack(world, out int entity))
            {
                if (world.GetPool<ShieldCock>().Has(entity)) world.GetPool<ShieldCock>().Del(entity);

                world.GetPool<EffectsContainer>().Get(entity).RemoveEffect(this);

                shieldEffectGO.SetActive(false);
            }
        }

        public override void Update(IEffect e)
        {

        }
    }
}
