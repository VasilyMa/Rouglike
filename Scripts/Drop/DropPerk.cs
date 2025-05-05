using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPerk : IDrop
{
    public Perk Perk;
    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition)
    {
        //I can make an interactive perk object, but I'm too lazy.
        return null;
    }
}
