using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class WeaponCreatorWindow : EditorWindow
{
    private static WeaponClass weapon;

    [MenuItem("Tools/Weapon Creator")]
    public static void Open()
    {
        GetWindow<WeaponCreatorWindow>("Weapon Creator");

        weapon = new();
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Creator", EditorStyles.boldLabel);
        
        SerializedObject serializedObject = new SerializedObject(weapon);

        SerializedProperty weaponNameProperty = serializedObject.FindProperty("weaponName");
        SerializedProperty weaponMeshProperty = serializedObject.FindProperty("weaponMesh");
        SerializedProperty damagesProperty = serializedObject.FindProperty("damage");
        SerializedProperty pushForceProperty = serializedObject.FindProperty("pushForce");
        SerializedProperty attackCountProperty = serializedObject.FindProperty("attackCount");
        SerializedProperty animationsProperty = serializedObject.FindProperty("weaponAttackAnimations");
        SerializedProperty animationsSpecProperty = serializedObject.FindProperty("weaponSpecAttackAnimations");
        SerializedProperty animationsMoveProperty = serializedObject.FindProperty("weaponMoveAnimations");
        SerializedProperty visualEffectsProperty = serializedObject.FindProperty("attackVisualEffects");
        SerializedProperty specVisualEffectsProperty = serializedObject.FindProperty("specAttackVisualEffects");
        SerializedProperty hitEffectsProperty = serializedObject.FindProperty("hitVisualEffects");
        SerializedProperty specHitEffectsProperty = serializedObject.FindProperty("specHitVisualEffects");
        SerializedProperty directionsTypeProperty = serializedObject.FindProperty("directionsType");

        EditorGUILayout.PropertyField(weaponNameProperty);
        EditorGUILayout.PropertyField(weaponMeshProperty);
        EditorGUILayout.PropertyField(pushForceProperty);
        EditorGUILayout.PropertyField(attackCountProperty);

        SerializedObject animationsObject = animationsProperty.serializedObject;
        SerializedObject animationsSpecObject = animationsSpecProperty.serializedObject;
        SerializedObject animationsMoveObject = animationsMoveProperty.serializedObject;
        SerializedObject damagesObject = damagesProperty.serializedObject;
        SerializedObject visualEffectsObject = visualEffectsProperty.serializedObject;
        SerializedObject specVisualEffectsObject = specVisualEffectsProperty.serializedObject;
        SerializedObject hitEffectsObject = hitEffectsProperty.serializedObject;
        SerializedObject specHitEffectsObject = specHitEffectsProperty.serializedObject;
        SerializedObject directionTypeObject = directionsTypeProperty.serializedObject;

        damagesProperty.arraySize = attackCountProperty.intValue;
        animationsProperty.arraySize = attackCountProperty.intValue;
        animationsSpecProperty.arraySize = 1;
        animationsMoveProperty.arraySize = 9;
        visualEffectsProperty.arraySize = attackCountProperty.intValue;
        specVisualEffectsProperty.arraySize = 1;
        hitEffectsProperty.arraySize = attackCountProperty.intValue;
        specHitEffectsProperty.arraySize = 1;
        directionsTypeProperty.arraySize = attackCountProperty.intValue;

        EditorGUILayout.LabelField("Damage");
        for (int i = 0; i < damagesProperty.arraySize; i++)
        {
            SerializedProperty damageProperty = damagesProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(damageProperty);
        }
        EditorGUILayout.LabelField("Attack Animations");
        for (int i = 0; i < animationsProperty.arraySize; i++)
        {
            SerializedProperty animationProperty = animationsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(animationProperty);
        }
        EditorGUILayout.LabelField("Spec Attack Animations");
        for (int i = 0; i < animationsSpecProperty.arraySize; i++)
        {
            SerializedProperty animationSpecProperty = animationsSpecProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(animationSpecProperty);
        }
        EditorGUILayout.LabelField("Move Animations");
        for (int i = 0; i < animationsMoveProperty.arraySize; i++)
        {
            SerializedProperty animationMoveProperty = animationsMoveProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(animationMoveProperty);
        }
        EditorGUILayout.LabelField("Direction Of Attack");
        for (int i = 0; i < directionsTypeProperty.arraySize; i++)
        {
            SerializedProperty directionTypeProperty = directionsTypeProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(directionTypeProperty);
        }
        EditorGUILayout.LabelField("Attack Visual Effects");
        for (int i = 0; i < visualEffectsProperty.arraySize; i++)
        {
            SerializedProperty visualEffectProperty = visualEffectsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(visualEffectProperty);
        }
        EditorGUILayout.LabelField("Spec Attack Visual Effects");
        for (int i = 0; i < specVisualEffectsProperty.arraySize; i++)
        {
            SerializedProperty specVisualEffectProperty = specVisualEffectsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(specVisualEffectProperty);
        }
        EditorGUILayout.LabelField("Hit Visual Effects");
        for (int i = 0; i < hitEffectsProperty.arraySize; i++)
        {
            SerializedProperty hitEffectProperty = hitEffectsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(hitEffectProperty);
        }
        EditorGUILayout.LabelField("Spec Hit Visual Effects");
        for (int i = 0; i < hitEffectsProperty.arraySize; i++)
        {
            SerializedProperty specHitEffectProperty = specHitEffectsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(specHitEffectProperty);
        }

        animationsObject.ApplyModifiedProperties();
        animationsSpecObject.ApplyModifiedProperties();
        animationsMoveObject.ApplyModifiedProperties();
        visualEffectsObject.ApplyModifiedProperties();
        specVisualEffectsObject.ApplyModifiedProperties();
        hitEffectsObject.ApplyModifiedProperties();
        specHitEffectsObject.ApplyModifiedProperties();
        damagesObject.ApplyModifiedProperties();
        directionTypeObject.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Save Weapon"))
        {
            SaveWeapon();
        }

    }

    private void SaveWeapon()
    {

        WeaponConfig newWeaponConfig = ScriptableObject.CreateInstance<WeaponConfig>();
        newWeaponConfig.KEY_ID = weapon.weaponName;
        newWeaponConfig.name = weapon.weaponName;
        newWeaponConfig.WeaponMesh = weapon.weaponMesh;
        /*newWeaponConfig.weaponAttackAnimations = weapon.weaponAttackAnimations;
        newWeaponConfig.weaponSpecAttackAnimations = weapon.weaponSpecAttackAnimations;
        newWeaponConfig.weaponMoveAnimations = weapon.weaponMoveAnimations;
        newWeaponConfig.attackVisualEffects = weapon.attackVisualEffects;
        newWeaponConfig.specAttackVisualEffects = weapon.specAttackVisualEffects;
        newWeaponConfig.hitVisualEffects = weapon.hitVisualEffects;
        newWeaponConfig.specHitVisualEffects = weapon.specHitVisualEffects;
        //newWeaponConfig.damage = weapon.damage;
        //newWeaponConfig.pushForce = weapon.pushForce;
        newWeaponConfig.attackCount = weapon.attackCount;*/
        //newWeaponConfig.directionsType = weapon.directionsType;
       
        string configPath = AssetDatabase.GenerateUniqueAssetPath("Assets/WeaponConfigs/" + newWeaponConfig.name + ".asset");
        AssetDatabase.CreateAsset(newWeaponConfig, configPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", "Weapon saved successfully.", "OK");
        weapon = new();
    }
}
#endif