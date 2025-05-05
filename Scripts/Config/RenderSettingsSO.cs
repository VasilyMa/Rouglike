using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "RenderSettingsSO", menuName = "RenderSettingsSO", order = 0)]
public class RenderSettingsSO : ScriptableObject {
    public Material SkyboxMaterial;
    public Light SunSource;
    [ColorUsageAttribute(true, false)]
    public Color RealtimeShadowColor;
    
    [Header("Environment Lighting")]
    public AmbientMode Source;
    [ColorUsageAttribute(false, true)]
    public Color SkyColor;
    [ColorUsageAttribute(false, true)]
    public Color EquatorColor;
    [ColorUsageAttribute(false, true)]
    public Color GroundColor;

    [Header("Environment Reflections")]
    public DefaultReflectionMode ReflectionSource;
    public int Resolution;
    public ReflectionCubemapCompression Compression;
    [Range(0f, 1f)]
    public float IntensityMultiplier;
    [Range(1, 5)]
    public int Bounces = 1;

    [Header("Other Settings")]
    public bool Fog;
    [ColorUsageAttribute(true, false)]
    public Color FogColor;
    public FogMode Mode;
    public float Start;
    public float End;
    public Texture2D HaloTexture;
    [Range(0f, 1f)]
    public float HaloStrength;
    public float FlareFadeSpeed;
    [Range(0f, 1f)]
    public float FlareStrength;

    [Space(10)]
    [Header("Directional Light Settings")]
    [ColorUsageAttribute(true, false)]
    public Color Color;
    public float Intensity;
    public float IndirectMultiplier;
    public Texture2D Cookie;
    public float CookieSize;
}



