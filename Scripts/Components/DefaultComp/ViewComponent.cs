using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    struct ViewComponent
    {
        //public Transform Transform;
        //public Collider Collider;
        //public Animator Animator;
        //public AnimationTypes AnimationType;
        public GameObject GameObject;
        //public GameObject AttackLine;
        //public UnitMB UnitMB;
       // public Rigidbody Rigidbody;
        //public AttackZone AttackZone;
        //public NavMeshAgent NavMeshAgent;
        //public SkinnedMeshRenderer SkinnedMeshRenderer;
        //public Rigidbody[] RagDollRigidBody;
        //public Collider[] RagDollCollider;
    }
    struct NavMeshComponent
    {
        public NavMeshAgent NavMeshAgent;
    }
    struct AttackComponent
    {
        public AttackZone AttackZoneReference;
        public GameObject AttackLine;
    }
    struct AnimatorComponent
    {
        public const string IsMove = "IsMove";
        public const string IsInAction = "IsInAction";
        public const string IsToughness = "IsToughness";
        public const string Dash = "Dash";
        public const string Attack1 = "Attack1";
        public const string Attack2 = "Attack2";
        public const string Attack3 = "Attack3";
        public const string SpecAttack1 = "SpecAttack";
        public const string SpecAttack2 = "SpecAttack2";
        public const string SpecAttack3 = "SpecAttack3";
        public const string HitFront = "HitFront";
        public const string HitBack = "HitBack";
        public const string HitRight = "HitRight";
        public const string HitLeft = "HitLeft";
        public const string Knockback = "Knockback";
        public const string GetUp = "GetUp";
        public const string Tired = "Tired";
        public const string IsTired = "IsTired";
        public const string Recovery = "Recovery";
        public const string IsPrepare = "IsPrepare";
        public const string Prepare = "Prepare";
        public const string Cast = "Cast";
        public const string Spawm = "Spawn";
        public const string Move = "Move";
        public const string CombatSlot1 = "CombatSlot1";
        public const string CombatSlot2 = "CombatSlot2";
        public const string CombatSlot3 = "CombatSlot3";

        public Animator Animator;
        public AnimationTypes AnimationType;
    }
    struct ColliderComponent
    {
        public Collider[] RagDollCollider;
        public Collider Collider;
    }
    struct RigidbodyComponent
    {
        public Rigidbody Rigidbody;
        public Rigidbody[] RagDollRigidBody;
    }
    struct MeshComponent
    {
        public SkinnedMeshRenderer[] SkinnedMeshRenderers;
    }
    struct TransformComponent
    {
        public Transform Transform;
    }
}