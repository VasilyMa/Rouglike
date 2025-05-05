using Client;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.VFX;

#if UNITY_EDITOR
using UnityEditor.Animations;
[UnityEditor.CustomEditor(typeof(AttackZoneService))]
public class AttackZoneEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {

        AttackZoneService attackZone = (AttackZoneService)target;

        UnityEditor.EditorGUILayout.LabelField("Attack Zone Mesh", UnityEditor.EditorStyles.boldLabel);

        if (GUILayout.Button("Save Mesh"))
        {
            attackZone.SaveMesh();
        }

       /* if (GUILayout.Button("Delete Mesh"))
        {
            attackZone.DeleteMesh();
        }*/

        base.OnInspectorGUI();

    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class AttackZoneService : MonoBehaviour
{
    [Space(10)]
    [Header("Property")]
    public AbilityBase AbilityBase;
    public AnimationClip Animation;
    public VisualEffectAsset VisualEffect;
    private MeshFilter WeaponMeshFilter;
    public Mesh WeaponMesh;
    private VisualEffect VFX;
    [HideInInspector] public GameConfig GameConfig;
    [Space(10)]
    [Header("Attack Zone Mesh")]
    [Range(0.1f, 20)] public float Radius;
    [Range(0.1f, 360)] public float Angle;
    public Vector3 meshPosition = Vector3.zero;
    public Quaternion meshRotation = Quaternion.identity;
    [HideInInspector] public int entityOwner;
    private int vertexDensity = 10;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;
    public List<Mesh> AttackZoneMeshes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!AbilityBase)
        {
            
            return;
        }

        if (!WeaponMeshFilter)
        {
            gameObject.transform.parent.GetComponentInChildren<WeaponMB>().GetComponent<MeshFilter>();
        }



        string folderPath = $"Assets/AttackZoneMeshes/{AbilityBase.name}";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string[] attackZoneMeshPaths = Directory.GetFiles(folderPath, $"{AbilityBase.name}**.asset");
        
        
        AttackZoneMeshes = new List<Mesh>();
        foreach (string meshPath in attackZoneMeshPaths)
        {
            Mesh mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);  
            if (!AttackZoneMeshes.Contains(mesh))
            {
                AttackZoneMeshes.Add(mesh);
            }
            else
            {
                
            }
        }
        /*if (AbilityBase.GetBaseData() is MainAbility mainAbility)
                mainAbility.SetAttackZoneMeshes(AttackZoneMeshes);*/
        //

        AnimatorController animController = (AnimatorController)gameObject.transform.parent.GetComponent<Animator>().runtimeAnimatorController;
        var layers = animController.layers;
        var states = layers[0].stateMachine.states;
        foreach (var state in states)
        {
            switch (state.state.name)
            {
                case "New State":
                    state.state.motion = Animation;
                    break;
            }

            if (VFX)
            {
                VFX = GetComponent<VisualEffect>();
                VFX.visualEffectAsset = VisualEffect;
            }
            if(WeaponMeshFilter && WeaponMesh) WeaponMeshFilter.sharedMesh = WeaponMesh;
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            meshFilter.sharedMesh = null;
            meshCollider.sharedMesh = null;
            CreateCircleSector(true);
            meshCollider.enabled = true;
        }
    }
#endif
    private List<int> _serviceEntityList = new List<int>();
    public void SetAttackZone(Mesh mesh)
    {
        AttackZoneDisable();
        meshCollider.sharedMesh = mesh;
    }
    public void AttackZoneEnable()
    {
        AttackZoneDisable();
        meshCollider.enabled = true;
    }
    public void AttackZoneDisable()
    {
        _serviceEntityList.Clear();
        meshCollider.enabled = false;
    }
    #region CreateSector

    private void CreateCircleSector(bool IsPlayer)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3 centerOffset = Vector3.zero;
        vertices.Add(meshPosition + centerOffset);

        int numVertices = Mathf.CeilToInt(Angle / vertexDensity) + 1;
        if (numVertices <= 1) return;
        if (IsPlayer)
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < numVertices; i++)
                {
                    float currentAngle = Mathf.Deg2Rad * (i * Angle / (numVertices - 1));
                    Vector3 vertex = new Vector3(Radius * Mathf.Cos(currentAngle), Radius * Mathf.Sin(currentAngle), k * 2);
                    vertex = meshRotation * vertex + meshPosition + centerOffset;
                    vertices.Add(vertex);
                }
            }
        }
        else
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < numVertices; i++)
                {
                    float currentAngle = Mathf.Deg2Rad * (i * Angle / (numVertices - 1));
                    Vector3 vertex = new Vector3(Radius * Mathf.Cos(currentAngle), Radius * Mathf.Sin(currentAngle), k * 2);
                    vertex = meshRotation * vertex + meshPosition + centerOffset;
                    vertices.Add(vertex);
                }
            }
        }

        for (int i = 0; i < numVertices - 1; i++)
        {
            int lower = i + 1;
            int upper = lower + numVertices;

            triangles.Add(0);
            triangles.Add(lower);
            triangles.Add(upper);

            triangles.Add(lower);
            triangles.Add(upper);
            triangles.Add(upper + 1);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        CreateCollider();
    }


    private void CreateCollider()
    {
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        collider.convex = true;
        collider.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        collider.isTrigger = true;
        meshCollider = collider;
        AttackZoneDisable();
    }
    #endregion
    #region UImesh
    public void SaveMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            string folderPath = $"Assets/AttackZoneMeshes/{AbilityBase.name}";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string baseFilePath = Path.Combine(folderPath, $"{AbilityBase.name}.asset");
            string filePath = baseFilePath;
            int fileIndex = 1;

            // ���������, ���� ���� � ����� �� ������ ��� ����������, � ��������� ����� � �����
            while (UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(filePath) != null)
            {
                filePath = Path.Combine(folderPath, $"{AbilityBase.name}_{fileIndex}.asset");
                fileIndex++;
            }


            UnityEditor.AssetDatabase.CreateAsset(meshFilter.sharedMesh, filePath);
            UnityEditor.AssetDatabase.SaveAssets();
            
            OnValidate();
        }

        else
        {
            
        }
    }


    /*public void DeleteMesh()
    {
        string folderPath = "Assets/AttackZoneMeshes";
        string filePath = Path.Combine(folderPath, $"{weaponConfig.name}.asset");
        weaponConfig.attackZoneMeshes.Remove(meshFilter.mesh);
        if (File.Exists(filePath))
        {
            AssetDatabase.DeleteAsset(filePath);
            AssetDatabase.SaveAssets();
            
        }
        else
        {
            
        }
    }
    *//*public void InsertMesh(string meshName)
    {
        string folderPath = "Assets/AttackZoneMeshes";
        string fileName = $"{meshFilter.mesh}.asset";
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath(filePath, typeof(Mesh));
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            
        }
        else
        {
            
        }
    }*/
    #endregion
    public enum WeaponAttackType
    {
        HammerFirstAttack,
        HammerSecondAttack,
        HammerThirdAttack,
        SwordFirstAttack,
        SwordSecondAttack,
        SwordThirdAttack,

        SpecialAttack,
        CastAttack,
    }

}
#endif