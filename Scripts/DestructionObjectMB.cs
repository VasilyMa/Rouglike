using Client;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Statement;

public class DestructionObjectMB : MonoBehaviour
{
    [HideInInspector] List<Rigidbody> RigidbodyChild;
    public float PushForce = 10;
    public bool isDestroyed = false;
    [HideInInspector] private bool IsValidate = false;
    void Start()
    {
        RigidbodyChild = GetComponentsInChildren<Rigidbody>().ToList();
        RigidbodyChild.Add(GetComponent<Rigidbody>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isDestroyed)
        {
            if (other.gameObject.GetComponent<AttackZone>())
            {
                Destruction(other);
            }
            if (other.gameObject.GetComponent<UnitMB>())
            {
                var unitMb = other.gameObject.GetComponent<UnitMB>();

                if (State.Instance.EcsRunHandler.World.GetPool<Dashing>().Has(unitMb._entity))
                {
                    Destruction(other);
                }
            }
        }
    }
    public void Destruction(Collider other)
    {
        foreach (var rigidbody in RigidbodyChild)
        {
            rigidbody.isKinematic = false;
            Vector3 pushDirection = (transform.position - other.transform.position);
            var dir = pushDirection.normalized;
            rigidbody.AddExplosionForce(PushForce, gameObject.transform.position - dir * 0.5f, 1, 0.01f, ForceMode.Impulse);
            ref var playSound = ref State.Instance.EcsRunHandler.World.GetPool<PlaySoundEvent>().Add(State.Instance.EcsRunHandler.World.NewEntity());
            playSound.eventReference = SoundEntity.Instance.GetPlaceHolderSound();
            playSound.SoundTransform = this.transform;
        }
        isDestroyed = true;
        GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
    }
//#if UNITY_EDITOR

//    private void OnValidate()
//    {
//        if (!gameObject.GetComponent<NavMeshObstacle>())
//        {
//            var NaveMeshParent = gameObject.AddComponent<NavMeshObstacle>();
//            NaveMeshParent.carving = true;
//        }
//        if (!gameObject.GetComponent<MeshCollider>())
//        {
//            var ColliderParent = gameObject.AddComponent<MeshCollider>();
//            ColliderParent.convex = true;
//        }
//        if (!gameObject.GetComponent<Rigidbody>())
//        {
//            var RigidbodyParent = gameObject.AddComponent<Rigidbody>();
//            RigidbodyParent.isKinematic = true;
//            RigidbodyParent.drag = 0.5f;
//        }

//        for (int i = 0; i < transform.childCount; i++)
//        {
//            if (!transform.GetChild(i).gameObject.GetComponent<MeshCollider>())
//            {
//                var ColliderParent = transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
//                ColliderParent.convex = true;
//            }
//            if (!transform.GetChild(i).gameObject.GetComponent<Rigidbody>())
//            {
//                var RigidbodyParent = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
//                RigidbodyParent.isKinematic = true;
//                RigidbodyParent.drag = 0.5f;
//            }
//        }
//        IsValidate = true;

//    }
//#endif
}
