using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using Client;
using AbilitySystem;
[System.Serializable]
public class WeaponClass : ScriptableObject
{
    public string weaponName;
    public Mesh weaponMesh;
    public AnimationClip[] weaponAttackAnimations;
    public AnimationClip[] weaponSpecAttackAnimations;
    public AnimationClip[] weaponMoveAnimations;
    public Mesh[] AttackZoneMeshs;
    public float[] damage;
    public float pushForce;
    public SourceParticle[] attackVisualEffects;
    public SourceParticle[] specAttackVisualEffects;
    public SourceParticle[] hitVisualEffects;
    public SourceParticle[] specHitVisualEffects;
    public int attackCount;
    public DirectionTypeF[] directionsType;
}
