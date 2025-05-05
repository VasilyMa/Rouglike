using System;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.Image;
using Leopotam.EcsLite;
using Client;
using Statement;

[CreateAssetMenu(fileName = "Perk", menuName = "Perk/New")]
public class Perk : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeReference] public ModifierTags ModifierTags;
    [Header("Conditions")] 
    [SerializeReference] public List<IPerkCondition> Conditions;
    
    [Header("Dispose result condition")] 
    [SerializeReference] public List<IDisposeResolvePerkCondition> DisposeConditions;
    [Header("Results of fulfilling conditions")] 
    [SerializeReference] public List<IPerkResolveEffect> Resolve;
    //todo resolve perks components
    [Header("For UI")]
    [SerializeField] public Sprite Icon;
    public Rarity Rarity;
    public string KEY_ID;
    public string Description;
    public string Name;
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrEmpty(name)) return;

        KEY_ID = name;

    }
    
}




