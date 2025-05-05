using System;
using System.Collections.Generic;
using Client;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AbilitySystem
{
    [System.Serializable]
    public class Ability
    {
        private bool _isPalyer;
        [ShowIf("_isPalyer")] public string Name;
        [ShowIf("_isPalyer")] public bool InstantAbility;
        public AbilityTypes AbilityType;
        [ShowIf("_isPalyer")] public bool TimerStartsOnPress;
        [ShowIf("_isPalyer")] public AbilityBase PreRequisite;
        [ShowIf("_isPalyer")] public Sprite IconAbility;
        [HideInInspector] public string PreRequisiteId;
        public InputActionReference InputActionReference;
        [HideIf("_isPalyer")] public AnimationCurve ProfitabilityDistance;
        [Tooltip("Basic block, компоненты вешаются на ентити абилки")]
        [SerializeReference] public List<BasicBlock> BasicBlocks;
        [Tooltip("Input block, замена precast / docast")]
        [SerializeReference] public List<InputBlock> InputBlocks;
        [Tooltip("FX block")]
        [SerializeReference] public List<TimeLineBlock> TimeLineBlocks;
        [Tooltip("Resolve block")]
        [SerializeReference] public List<ResolveBlock> ResolveBlocks;
        public Action<bool> OnStatusChange;
        [HideInInspector] public bool IsCooldown { get; set; }
        [HideInInspector] public bool IsPreCasted = false;
        [HideInInspector] public float Cooldown;
        [HideInInspector] public float MaxCooldown;
        [HideInInspector] public float CostRequirements;


        public Ability(Ability data)
        {
            if (data is not null)
            {
                Name = data.Name;
                BasicBlocks = data.BasicBlocks;
                InputBlocks = data.InputBlocks;
                TimeLineBlocks = data.TimeLineBlocks;
                ResolveBlocks = data.ResolveBlocks;
                IconAbility = data.IconAbility;
            }
        }
        public int GetDamage()
        {
            int value = 0;

            foreach (var resolve in ResolveBlocks)
            {
                foreach (var component in resolve.Components)
                {
                    if (component is DamageEffect damageEffect)
                    {
                        value += (int)damageEffect.DamageValue;
                    }
                }
            }

            return value;
        }
        public int GetCooldown()
        {
            return 0;// A NAXYI?
        }
        public void Update(bool isPalyer)
        {
            _isPalyer = isPalyer;
           // if(!isPalyer) TimerStartsOnPress = false;
        }
    }
    public enum AbilityTypes
    {
        Attack, Utility, Defence, Terrorize, Spawn, FromPlace
    }
}
