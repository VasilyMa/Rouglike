using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Statement;

public class DashParticle : MonoBehaviour
{
    [SerializeField]private VisualEffect _effect;
    private float _time = 0.3f;
    public void PlayDashVFX()
    {
        _effect.Play();
    }
    public void SetVelocityProperty(Vector3 value)
    {
        _effect.SetVector3("MeshVelocity", value);
    }
    public void Stop()
    {
        _effect.Stop();
    }
}
