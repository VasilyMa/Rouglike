using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class MeshCreator : MonoBehaviour
{
    private MeshFilter _mf, _lmf, _rmf;
    private MeshRenderer _mr, _lmr, _rmr;
    public CubicBezier3D cubic;
    [SerializeField] private GameObject LeftRailing, RightRailing;

    public SplineDrawer DrawerInstance;
    
    public void GenerateBridge(RoomGOConfig biomeConfig,Transform start, Transform end, SplineDrawer drawer)
    {
        _mf = GetComponent<MeshFilter>();
        _mr = GetComponent<MeshRenderer>();
        _lmf = LeftRailing.GetComponent<MeshFilter>();
        _lmr = LeftRailing.GetComponent<MeshRenderer>();
        _rmf = RightRailing.GetComponent<MeshFilter>();
        _rmr = RightRailing.GetComponent<MeshRenderer>();


        //todo GetPosition from biome
        Vector2[] mainMeshHalfPositions = biomeConfig.MainMeshHalfPositions;
        Vector2[] railingHalfPositions = biomeConfig.RailingHalfPositions;
        // Vector2[] mainMeshHalfPositions = new Vector2[]
        // {
        //     new Vector2(-5f, 0f)
        // };
        // Vector2[] railingHalfPositions = new Vector2[]
        // {
        //     new Vector2(0f, 0f),
        //     new Vector2(1f, -0.4f),
        //     new Vector2(1f, -0.4f),
        //     new Vector2(2f, -1f),
        //     new Vector2(2f, -1f),
        //     new Vector2(3f, -1.8f),
        //     new Vector2(3f, -1.8f),
        //     new Vector2(3.8f, -2.6f),
        //     new Vector2(3.8f, -2.6f),
        //     new Vector2(4.9f, -3.3f),
        //     new Vector2(4.9f, -3.3f),
        //     new Vector2(6f, -3.8f),
        //     new Vector2(6f, -3.8f),
        //     new Vector2(7.3f, -4.3f),
        //     new Vector2(7.3f, -4.3f),
        //     new Vector2(8.4f, -4.7f),
        //     new Vector2(8.4f, -4.7f),
        //     new Vector2(9.6f, -5f)
        // };
        this.transform.position = start.position;
        SplineDrawer drawerInstance = Instantiate(drawer, this.transform.position, Quaternion.identity);
        drawerInstance.transform.parent = this.transform;
        cubic = new CubicBezier3D(drawerInstance.InitSpline(start, end), 20);
        drawerInstance.DrawSpline();
        if (drawerInstance.GenerateMesh)
        {
            CreateMesh(_mf, _mr, mainMeshHalfPositions, new Vector2(0f, 0f), cubic, "mainMesh", drawerInstance.mainMaterial);
            this.gameObject.AddComponent<MeshCollider>();
        }
        if (drawerInstance.GenerateRailings)
        {
            CreateMesh(_lmf, _lmr, railingHalfPositions, new Vector2(-5f, 0f), cubic, "leftRailings", drawerInstance.additionalMaterial);
            CreateMesh(_rmf, _rmr, railingHalfPositions, new Vector2(5f, 0f), cubic, "rightRailings", drawerInstance.additionalMaterial);
        }
        DrawerInstance = drawerInstance;
    }
    private void CreateMesh(MeshFilter mf, MeshRenderer mr, Vector2[] shapeVertices, Vector2 splineOffset, CubicBezier3D cubicBezier, string meshName, Material material)
    {
        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();
        Mesh mesh = mf.sharedMesh;
        ExtrudeShape shape = MirrorShapeWithOffset(shapeVertices, splineOffset);
        mesh.name = meshName;
        Extrude(mesh, shape, cubicBezier.path);
        mr.sharedMaterial = material;
    }
    private ExtrudeShape MirrorShapeWithOffset(Vector2[] halfPositions, Vector2 offset = new Vector2())
    {
        List<Vector2> halfPositionsList = halfPositions.ToList();
        // Mirror positions to end of a list
        for (int i = halfPositions.Length - 1; i >= 0; i--)
        {
            halfPositionsList.Add(new Vector2(-halfPositions[i].x, halfPositions[i].y)); 
        }
        for (int i = 0; i < halfPositionsList.Count; i++)
        {
            halfPositionsList[i] += offset;
        }
        int[] lines = new int[halfPositionsList.Count];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = i;
        }
        return new ExtrudeShape(halfPositionsList.ToArray(), lines);
    }
    private Vector3 GetPoint(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return pts[0] * (omt2 * omt) +
            pts[1] * (3f * omt2 * t) +
            pts[2] * (3f * omt * t2) +
            pts[3] * (t2 * t);
    }

    private Vector3 GetTangent(Vector3[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
            pts[0] * (-omt2) +
            pts[1] * (3 * omt2 - 2 * omt) +
            pts[2] * (-3 * t2 + 2 * t) +
            pts[3] * (t2);
        return tangent.normalized;
    }
    private Vector2 GetTangent2D(Vector2[] pts, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector2 tangent =
            pts[0] * (-omt2) +
            pts[1] * (3 * omt2 - 2 * omt) +
            pts[2] * (-3 * t2 + 2 * t) +
            pts[3] * (t2);
        return tangent.normalized;
    }

    private Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up)
    {
        Vector3 tng = GetTangent(pts, t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }

    public void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path)
    {
        int vertsInShape = shape.vertices.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments; // *2?
        int triIndexCount = triCount * 3;
        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        for (int i = 0; i < path.Length; i++)
        {
            int offset = i * vertsInShape;
            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.vertices[j].position);
                normals[id] = path[i].LocalToWorldDirection(shape.vertices[j].normal);
                uvs[id] = new Vector2(shape.vertices[j].uCoord, (float)i / ((float)edgeLoops));
            }
        }
        int ti = 0;
        for (int i  = 0; i  < segments; i ++)
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.lines.Length; l += 2)
            {
                int a = offset + shape.lines[l]; // 0 + 0 = 0
                int b = offset + shape.lines[l] + vertsInShape; // 0 + 0 + 5 = 5
                int c = offset + shape.lines[l + 1]; // 0 + 1 = 1
                int d = offset + shape.lines[l + 1] + vertsInShape; // 0 + 1 + 5 = 6
                triangleIndices[ti] = a; ti++;
                triangleIndices[ti] = b; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = d; ti++;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = b; ti++; // [5, 0, 1, 1, 6, 5] counterclockwise => 
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;
    }
    private void CalcLengthTableInto(float[] array, CubicBezier3D bezier)
    {
        array[0] = 0f;
        float totalLength = 0f;
        Vector3 prev = bezier.p0;
        for (int i = 1; i < array.Length; i ++)
        {
            float t = ((float)i) / (array.Length - 1);
            Vector3 point = bezier.GetPoint(t);
            float diff = (prev - point).magnitude;
            totalLength += diff;
            array[i] = totalLength;
            prev = point;
        }
    }
}
public class CubicBezier3D
{
    public Vector3 p0, p1, p2, p3;
    public OrientedPoint[] path;
    public CubicBezier3D(Vector3[] points, int pointsCount)
    {
        this.p0 = points[0]; 
        this.p1 = points[1];
        this.p2 = points[2];
        this.p3 = points[3];
        path = new OrientedPoint[pointsCount];
        float tOffset = 1f / (float)(pointsCount - 1);
        float t = 0f;
        for (int i = 0; i < pointsCount; i ++)
        {
            path[i] = new OrientedPoint(GetPoint(t), GetOrientation3D(t, Vector3.up));
            t += tOffset;
        }
    }
    public Vector3 GetPoint(float t) //todo COLLIDER
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return p0 * (omt2 * omt) +
            p1 * (3f * omt2 * t) +
            p2 * (3f * omt * t2) +
            p3 * (t2 * t);
    }
    public Vector3 GetTangent(float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
            p0 * (-omt2) +
            p1 * (3 * omt2 - 2 * omt) +
            p2 * (-3 * t2 + 2 * t) +
            p3 * (t2);
        return tangent.normalized;
    }
    private Vector3 GetNormal3D(float t, Vector3 up)
    {
        Vector3 tng = GetTangent(t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
     
        return Vector3.Cross(tng, binormal);
    }
    private Quaternion GetOrientation3D(float t, Vector3 up)
    {
        Vector3 tng = GetTangent(t);
        Vector3 nrm = GetNormal3D(t, up);
        return Quaternion.LookRotation(tng, nrm);
    }
}

public static class FloatArrayExtensions
{
    public static float Sample(this float[] floatArray, float t)
    {
        int count = floatArray.Length;
        if (count == 0)
        {
            
            return 0;
        }
        if (count == 1)
        {
            return floatArray[0];
        }
        float iFloat = t * (count - 1);
        int idLower = Mathf.FloorToInt(iFloat);
        int idUpper = Mathf.FloorToInt(iFloat + 1);
        if (idUpper >= count)
            return floatArray[count - 1];
        if (idLower < 0)
            return floatArray[0];
        return Mathf.Lerp(floatArray[idLower], floatArray[idUpper], iFloat - idLower);
    }
}
public struct ExtrudeShape
{
    public Vertice2D[] vertices;
    public int[] lines;
    public ExtrudeShape(Vector2[] points, int[] lines)
    {
        this.lines = lines;
        List<Vertice2D> vertices = new List<Vertice2D>();
        float cumulativeDistance = 0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 p1 = points[i];
            Vector2 p2 = points[i + 1];
            cumulativeDistance += (p2 - p1).magnitude;
        }
        float uCoord = 0f;
        for (int i = 0; i <= lines.Length - 2; i += 2)
        {
            Vector2 startPoint = points[lines[i]];
            Vector2 endPoint = points[lines[i + 1]];
            Vector2 line = endPoint - startPoint;
            Vector2 lineNormal = new Vector2(-line.y, line.x).normalized;
            Vertice2D startVertice = new Vertice2D();
            startVertice.position = startPoint;
            startVertice.normal = lineNormal;
            startVertice.uCoord = uCoord;
            uCoord += line.magnitude / cumulativeDistance;
            Vertice2D endVertice = new Vertice2D();
            endVertice.position = endPoint;
            endVertice.normal = lineNormal;
            endVertice.uCoord = uCoord;
            vertices.Add(startVertice);
            vertices.Add(endVertice);
        }
        this.vertices = vertices.ToArray();
    }
}

public struct Vertice2D
{
    public Vector2 position;
    public Vector2 normal;
    public float uCoord;
}

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public OrientedPoint(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
    public Vector3 LocalToWorld(Vector3 point)
    {
        return position + rotation * point;
    }
    public Vector3 WorldToLocal(Vector3 point)
    {
        return Quaternion.Inverse(rotation) * (point - position);
    }
    public Vector3 LocalToWorldDirection(Vector3 dir)
    {
        return rotation * dir;
    }
}