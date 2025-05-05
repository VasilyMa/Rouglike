using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition", menuName = "Config/Condition")]
public class ConditionSettings : ScriptableObject
{
    public int MaxPoint;
    [SerializeReference] public Condition _condition;
    [SerializeReference] public List<IConditionComponent> Components;
}
