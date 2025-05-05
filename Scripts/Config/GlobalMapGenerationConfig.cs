using AbilitySystem;
using UnityEngine;
using Client;

[System.Serializable]
public class GlobalMapGenerationConfig
{
    public int BiomCount = 1;
    public int MaxLinkCount = 3;
    public int Radius = 2;
    public int EnterCount = 3;
    [Header("Map Size")]
    public int MaxWidth = 10;
    public int MaxLength = 10;
}