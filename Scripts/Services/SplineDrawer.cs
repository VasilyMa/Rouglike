using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineDrawer : MonoBehaviour
{
    private SplineContainer _sc;
    private SplineInstantiate[] _si;
    public Material mainMaterial;
    public Material additionalMaterial;
    public bool GenerateMesh;
    public bool GenerateRailings;
    public bool GenerateProps;
    private Spline _spline;
    private float _bridgeWidth = 10f;
    [SerializeField] private float _minimalLerpValue;
    //TODO 2D shape in Scriptable Objects
    public void UpdateInstance()
    {
        foreach (SplineInstantiate si in _si)
        {
            si.enabled = true;
        }
    }
    public Vector3[] InitSpline(Transform startPoint, Transform endPoint)
    {
        _si = GetComponents<SplineInstantiate>();
        _sc = GetComponent<SplineContainer>();
        //_si = GetComponents<SplineInstantiate>();
        float3 startPos = new float3(0f, 0f, 0f);
        float3 endPos = new float3(endPoint.position - startPoint.position);
        float3 projection = Vector3.Project(endPos, -startPoint.forward);
        Quaternion lookRotation = Quaternion.LookRotation(-startPoint.forward);
        Vector3 positionOffset = projection;
        float magnitude = positionOffset.magnitude;
        float minOffset = Mathf.Min(endPos.x, endPos.z);
        if (minOffset <= _bridgeWidth * 2)
        {
            float multiplier = Mathf.Lerp(_minimalLerpValue, 1, minOffset / (_bridgeWidth * 2f));
            magnitude *= multiplier;
            projection *= multiplier;
            
        }
        _spline = new Spline();
        BezierKnot p0 = new BezierKnot(startPos, new float3(), new float3(0f, 0f, magnitude), lookRotation);
        BezierKnot p3 = new BezierKnot(endPos, -new float3(0f, 0f, magnitude), new float3(), lookRotation);
        _spline.Add(p0);
        _spline.Add(p3);
        DisableSplineInstantiate();
        return new Vector3[]
        {
            p0.Position, p0.Position + projection, p3.Position - projection, p3.Position
        };
    }
    public void DisableSplineInstantiate()
    {
        foreach (SplineInstantiate si in _si)
        {
            si.enabled = false;
        }
    }
    public void DrawSpline()
    {
        _sc.Spline = _spline;
    }

}