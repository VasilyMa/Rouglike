using System.Collections.Generic;

namespace Client 
{
    struct StaminaComponent : IStat
    {
        private float _base;
        private float _value;
        private List<float> _modifiers;

        public float Add(float value)
        {
            _value += value;
            _value = _value >= GetMaxValue() ? GetMaxValue() : _value;
            return _value;
        }

        public float AddModifier(float value)
        {
            _modifiers.Add(value);

            _value += value;

            _value = _value > GetMaxValue() ? GetMaxValue() : value;    

            return _value;
        }

        public float GetMaxValue()
        {
            float result = _base;

            _modifiers.ForEach(modifier => result += modifier);

            return result;
        }

        public float GetValue()
        {
            return _value;
        }

        public void Init(float value)
        {
            _base = value;
            _value = value;
            _modifiers = new List<float>();
        }

        public float RemoveModifier(float value)
        {
            _modifiers.Remove(value);

            _value -= value;

            _value = _value < 0 ? 0 : _value;

            return _value;
        }

        public float Subtract(float value)
        {
            _value -= value;

            _value = (_value < 0) ? 0 : _value;

            return _value;
        }
    }
}