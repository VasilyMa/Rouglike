using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ViewConfig", menuName = "Config/View")]
public class ViewConfig : Config
{
    [Header("Velues")]
    [SerializeReference] public IDrop[] Effygies;
    [SerializeReference] public IDrop[] Favours;
    public List<RelicView> RelicView;
    public int FavourTierStep;

    [Space(10f)]
    [Header("View")]
    public VisualDamageConfig VisualDamageConfig;
    public SlowVisualConfig SlowVisualConfig;

    public Material DisolveMaterial;

    public GameObject TeleGO;
    public GameObject MegaTeleGO;


    public override IEnumerator Init()
    {
        yield return null;
    }
}
[System.Serializable]
public class RelicView
{
    public Rarity Rarity;
    public InteractiveRelicObject InteractiveRelicObject;
}