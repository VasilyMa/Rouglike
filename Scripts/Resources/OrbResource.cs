using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Orb", menuName = "Resources/NewOrb")]
[System.Serializable]
public class OrbResource : ScriptableObject, ISerializationCallbackReceiver, IDissolvable
{
    [Header("������ ��������� ������")]
    public List<AbilityBase> abilities;
    public Material NeutralMaterial;
    public Material PoisonMaterial;
    public Material FrostMaterial;
    public Material FireMaterial;

    [ReadOnlyInspector] public string KEY_ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    public InteractiveOrbObject interactiveOrbObject;


    public void InvokeOrb()
    {

    }

   
    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
        }
    }

    public AbilityBase GetRandomAbility()
    {
        if (abilities == null || abilities.Count == 0)
        {
            
            return null;
        }

        int index = Random.Range(0, abilities.Count);

        var randomAbility = abilities[index];
        // if randomAbility.ModifierTags   =>   mat = modifier Material
        return randomAbility;
    }

    public void Dissolve()
    {
        PlayerEntity.Instance.Currency.SkillShard++;
        //to do add resource
    }
}
