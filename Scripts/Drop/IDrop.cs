using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrop 
{
    public InteractiveObject DropItem(Vector3 DropPosition, Vector3 EndPosition);
}
