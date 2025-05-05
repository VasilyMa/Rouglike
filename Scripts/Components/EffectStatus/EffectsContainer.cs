using AbilitySystem;

using Leopotam.EcsLite;

using System.Collections.Generic;

namespace Client {
    struct EffectsContainer
    {
        private List<IEffect> _effects;
        
        public void Init()
        {
            _effects = new List<IEffect>();
        }

        public void Run()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Run();
            }
        }

        public void AddEffect(IEffect e, EcsPackedEntity owner, EcsPackedEntity sender)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if (e.GetId() == _effects[i].GetId())
                {
                    _effects[i].Update(e);
                    return;
                }
            }

            e.AddEffect(owner, sender);

            _effects.Add(e);
        }

        public void RemoveEffect(IEffect effect) 
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Equals( effect))
                {
                    _effects.RemoveAt(i);
                }
            }
        }
    }
}