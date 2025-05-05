using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXOnBodyMB : MonoBehaviour
{
    public int IndexVFX;
    public SourceParticle sourseParticle;
    private SourceParticle _createdSourseParticle;
    private void Start()
    {
        _createdSourseParticle = Instantiate(sourseParticle,transform);
        _createdSourseParticle.transform.position = transform.position;
        _createdSourseParticle.transform.rotation = transform.rotation;
        Dispose();
        FindPhysicsUnitMB(transform);
    }
    private void FindPhysicsUnitMB(Transform transformChild)
    {
        if (transformChild is null) return;
        if (!transformChild.TryGetComponent<PhysicsUnitMB>(out var physicsUnitMB))
        {
            FindPhysicsUnitMB(transformChild.parent);
            return;
        }
        else
        {
            physicsUnitMB._VFXOnBody.Add(this);
        }

    }
    public void Play()
    {
        _createdSourseParticle.gameObject.SetActive(true);
        _createdSourseParticle.GetEffect().Play();
    }
    public void Dispose()
    {
        _createdSourseParticle.GetEffect().Stop();
        _createdSourseParticle.gameObject.SetActive(false);
    }
}