using UnityEngine;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Client
{
    struct WeaponComponent
    {
        public string weaponName;
        public float pushForce;
        public DirectionTypeF[] directionsType;
        public AbilityBase AbilitySecondaryBase;
        public AbilityBase AbilityMainBase;
        public List<AbilityBase> Abilities;

        public void Init(AbilityUnitMB unitMB)
        {
            weaponName = unitMB.WeaponConfig.KEY_ID;
            if (unitMB.WeaponConfig.WeaponMesh is not null)
            {
                if (unitMB.WeaponMeshFilter)
                    unitMB.WeaponMeshFilter.sharedMesh = unitMB.WeaponConfig.WeaponMesh;
            }

            Abilities = new List<AbilityBase>(unitMB.WeaponConfig.Abilities);
            unitMB.Animator.runtimeAnimatorController = unitMB.WeaponConfig.AnimatorOverrideController;

            //�������� ������, ������� �������������,
        }
    }
}