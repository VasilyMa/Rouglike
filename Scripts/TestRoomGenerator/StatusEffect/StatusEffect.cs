using Client;
using Leopotam.EcsLite;
using UnityEngine;

using Statement;

namespace AbilitySystem
{
    public abstract class StatusEffect : IEffect
    {
        protected EcsPackedEntity _ownerEntity;
        protected EcsPackedEntity _senderEntity;
        protected string _status_id;
        [SerializeField] protected MemberTypes _attachEffect;
        [SerializeField] protected float _lifeTime;
        public abstract void Update(IEffect e);
        public abstract void AddEffect(EcsPackedEntity owner, EcsPackedEntity sender);
        public abstract IEffect Clone();
        public abstract void RemoveEffect();
        public virtual string GetId()
        {
            return _status_id;
        }
        public virtual void Run()
        {
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0)
            {
                if(_ownerEntity.Unpack(BattleState.Instance.EcsRunHandler.World, out int entity))
                {
                    if (BattleState.Instance.EcsRunHandler.World.GetPool<PushComponent>().Has(entity))
                    {
                        BattleState.Instance.EcsRunHandler.World.GetPool<PushComponent>().Del(entity);
                    }
                }
                RemoveEffect();

            }
        }

        public virtual float GetLifeTime()
        {
            return _lifeTime;
        }
    }
}
