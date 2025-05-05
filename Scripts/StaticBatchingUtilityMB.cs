using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class StaticBatchingUtilityMB : MonoBehaviour
{
    public List<Mesh> _mesh = new();
    public List<MeshFilter> _meshFilter = new();
    private void OnValidate()
    {
        _meshFilter.Clear();
        _mesh.Clear();
        _meshFilter = this.GetComponentsInChildren<MeshFilter>().ToList();
        _meshFilter.RemoveAt(0);
        foreach(var mf in _meshFilter)
        {
            _mesh.Add(mf.sharedMesh);
        }
    }
    private void Start()
    {
        int i = 0;
    }
    public void StaticBatchingUtilityFunc()
    {
        StaticBatchingUtility.Combine(this.gameObject);
        return;
        CombineMeshes();
        var meshFilters = this.GetComponentsInChildren<MeshFilter>().ToList();
        meshFilters.RemoveAt(0);
        foreach (var filter in meshFilters)
        {
            if (filter != null && filter.gameObject != this.gameObject)
                filter.gameObject.SetActive(false);
        }
    }
    public void CombineMeshes()
    {
        List<CombineInstance> combine = new();
        Transform parentTransform = this.transform;
        var meshFilters = this.GetComponentsInChildren<MeshFilter>().ToList();
        meshFilters.RemoveAt(0);
        int i = 0;
        foreach (var meshF in _mesh)
        {
            if (meshF == null)
            {
                i++;
                continue;

            }
            if (meshF.vertices.Length == 0)
            { 
                i++;
                continue;
            }


            var newCombine = new CombineInstance();
            newCombine.mesh = meshF;
            newCombine.transform = parentTransform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
            combine.Add(newCombine);
            i++;
        }
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine.ToArray());
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.GetComponent<MeshRenderer>().sharedMaterial = transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial;
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
