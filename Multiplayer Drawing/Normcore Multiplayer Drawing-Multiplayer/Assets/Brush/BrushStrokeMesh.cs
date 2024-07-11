using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushStrokeMesh : MonoBehaviour {
    [SerializeField]
    private float _brushStrokeWidth = 0.05f;

    private Mesh _mesh;

    private List<Vector3> _vertices;
    private List<Vector3> _normals;

    private bool _skipLastRibbonPoint;
    public  bool  skipLastRibbonPoint { get { return _skipLastRibbonPoint; } set { if (value == _skipLastRibbonPoint) return; _skipLastRibbonPoint = value; UpdateGeometry(); } }

    private void Awake() {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        _mesh = filter.mesh;

        _vertices = new List<Vector3>();
        _normals  = new List<Vector3>();

        // In addition to clearing the ribbon, this adds a ribbon point that we'll move each frame to match the
        // brush tip position so that the brush stroke appears to paint continuously.
        ClearRibbon();
    }

    // Insert a new ribbon point just before the final ribbon point
    public void InsertRibbonPoint(Vector3 position, Quaternion rotation) {
        // Calculate vertices + normal for ribbon point
        Vector3 p1;
        Vector3 p2;
        Vector3 normal;
        CalculateVerticesAndNormalForRibbonPoint(position, rotation, _brushStrokeWidth, out p1, out p2, out normal);

        // Insert into vertices array
        _vertices.Insert(_vertices.Count-4, p1);
        _vertices.Insert(_vertices.Count-4, p2);
        _vertices.Insert(_vertices.Count-4, p1);
        _vertices.Insert(_vertices.Count-4, p2);

        // Insert into normals array
        _normals.Insert(_normals.Count-4,  normal);
        _normals.Insert(_normals.Count-4,  normal);
        _normals.Insert(_normals.Count-4, -normal);
        _normals.Insert(_normals.Count-4, -normal);

        // Update the mesh
        UpdateGeometry();
    }

    public void UpdateLastRibbonPoint(Vector3 position, Quaternion rotation) {
        // Calculate vertices + normal for ribbon point
        Vector3 p1;
        Vector3 p2;
        Vector3 normal;
        CalculateVerticesAndNormalForRibbonPoint(position, rotation, _brushStrokeWidth, out p1, out p2, out normal);

        int lastIndex = _vertices.Count-4;

        // Update vertices
        _vertices[lastIndex]   = p1;
        _vertices[lastIndex+1] = p2;
        _vertices[lastIndex+2] = p1;
        _vertices[lastIndex+3] = p2;

        // Update normals
        _normals[lastIndex]   =  normal;
        _normals[lastIndex+1] =  normal;
        _normals[lastIndex+2] = -normal;
        _normals[lastIndex+3] = -normal;

        // Update the mesh
        UpdateGeometry();
    }

    public void ClearRibbon() {
        // Clear vertices & normals
        _vertices.Clear();
        _normals.Clear();

        // Create last ribbon point
        _vertices.Add(Vector3.zero);
        _vertices.Add(Vector3.zero);
        _vertices.Add(Vector3.zero);
        _vertices.Add(Vector3.zero);
        _normals.Add(Vector3.zero);
        _normals.Add(Vector3.zero);
        _normals.Add(Vector3.zero);
        _normals.Add(Vector3.zero);

        // Update the mesh
        UpdateGeometry();
    }

    private void CalculateVerticesAndNormalForRibbonPoint(Vector3 position, Quaternion rotation, float width, out Vector3 p1, out Vector3 p2, out Vector3 normal) {
        p1     = position + rotation * new Vector3(-width/2.0f, 0.0f, 0.0f);
        p2     = position + rotation * new Vector3( width/2.0f, 0.0f, 0.0f);
        normal = rotation * Vector3.up;
    }

    private void UpdateGeometry() {
        int numberOfVertices = _vertices.Count;
        if (skipLastRibbonPoint)
            numberOfVertices -= 4;

        if (numberOfVertices < 8) {
            _mesh.vertices  = new Vector3[0];
            _mesh.normals   = new Vector3[0];
            _mesh.triangles = new int[0];

            _mesh.RecalculateBounds();

            return;
        }

        // Would probably make sense to just generate the new triangles rather than regenerating all of them all of the time.

        int numberOfSegments  = numberOfVertices/4 - 1;
        int numberOfTriangles = numberOfSegments * 4; // Two on the front side, two on the back.

        int[] triangles = new int[numberOfTriangles*3];
        for (int i = 0; i < numberOfSegments; i++) {
            // Front
            int p1 = i*4;
            int p2 = i*4+1;
            int p3 = i*4+4;
            int p4 = i*4+5;

            // Back
            int p1b = i*4+2;
            int p2b = i*4+3;
            int p3b = i*4+6;
            int p4b = i*4+7;

            // Front
            triangles[i*12]   = p1;
            triangles[i*12+1] = p2;
            triangles[i*12+2] = p3;
            triangles[i*12+3] = p2;
            triangles[i*12+4] = p4;
            triangles[i*12+5] = p3;

            // Back
            triangles[i*12+6]  = p1b;
            triangles[i*12+7]  = p3b;
            triangles[i*12+8]  = p2b;
            triangles[i*12+9]  = p2b;
            triangles[i*12+10] = p3b;
            triangles[i*12+11] = p4b;
        }

        _mesh.vertices  = _vertices.ToArray();
        _mesh.normals   = _normals.ToArray();
        _mesh.triangles = triangles;

        _mesh.RecalculateBounds();
    }
}
