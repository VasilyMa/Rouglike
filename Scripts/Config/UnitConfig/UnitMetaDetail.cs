using Client;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitMetaDetail : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float SlowSpeed;
    [Range(0, 3)] public float HitOffset;
    public GameObject Unit;
    public float SpawnDelay;

    public float MaxValueToughness;
    public float DelayRecoveryToughness;
    public bool Percent = false;
    public float SpeedRecovery;

    public SourceParticle ParticleSpawn; 
    [Space(10)]
    public AIProfile AIProfile;
    public Material Material;
    public WeaponConfig WeaponConfig;
    public Mesh MeshEnemy;

    public DropConfig DropConfig;
    public float GetSpeedRecovery()
    {
        if(Percent) return SpeedRecovery / 100 * MaxValueToughness;
        return SpeedRecovery;
    }
}

