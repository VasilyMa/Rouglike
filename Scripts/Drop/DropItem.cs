using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    [SerializeReference] public IDrop dropItem;
    public float Weight;
    public bool isOnce;
}
