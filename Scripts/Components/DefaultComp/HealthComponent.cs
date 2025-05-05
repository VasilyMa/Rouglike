using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct HealthComponent 
    {
        public float MaxValue;
        public float CurrentValue;
        public List<float> Modification;
        public List<float> ArmorModification;
        //todo SASHA resist
        public void Init(float current, float max)
        {
            CurrentValue = current;
            MaxValue = max;
            Modification = new List<float>();
            ArmorModification = new List<float>();
        }
        public float GetCurrent()
        {
            return CurrentValue;
        }
        public float GetMaxValue()
        {
            return MaxValue;
        }
        public float TakeDamageReturnCurrent(float damage)
        {
            CurrentValue -= damage;
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
            return CurrentValue;
        }
        public float TakeHeal(float heal)
        {
            CurrentValue += heal;
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
            return CurrentValue;
        }
        public float GetNewDamage(float damage)
        {
            float dmg = damage;
            foreach(var armor in ArmorModification)
            {
                dmg *= (1 - armor);
            }
            return dmg;
        }
        public void AddArmorModification(float value)
        {
            ArmorModification.Add(value);
        }
    }
}