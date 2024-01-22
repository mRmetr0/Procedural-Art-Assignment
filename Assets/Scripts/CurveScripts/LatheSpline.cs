using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LatheSpline : MeshCreator
{
    [SerializeField] private int curveAmount = 7;
    [SerializeField] private bool modifySharedMesh = false;
    
    private int getIndex(int x, int y, int height) {
        return y + x * height;
    }
    
    public override void RecalculateMesh()
    {
        Curve curve = GetComponent<Curve>();
        if (curve == null)
            return;
        List<Vector3> vertices = curve.points;
        MeshBuilder builder = new MeshBuilder();

        int vertexCount = vertices.Count;
        int curveCount = curveAmount;

        for (int curveIndex = 0; curveIndex <= curveCount; curveIndex++) {
            Quaternion rotation = Quaternion.Euler(0, curveIndex * 360.0f/curveCount, 0);

            for (int vertexI = 0; vertexI < vertexCount ; vertexI++)
            {
                Vector3 vertex = new Vector3(vertices[vertexI].x, vertices[vertexI].y, 0);
                Vector2 uv = new Vector2(curveIndex * 1f/curveCount, vertexI * 1f/vertexCount);
                vertex = rotation * vertex;
                builder.AddVertex(vertex, uv);
            }
        }

        // Generate Quads:
        for (int curveIndex = 1; curveIndex <= curveCount; curveIndex++)
        {
            for (int vertexIndex = 1; vertexIndex < vertexCount; vertexIndex++)
            {
                //generate 4 vertices (quad):
                int v0 = getIndex(curveIndex - 1, vertexIndex - 1, vertexCount);
                int v1 = getIndex(curveIndex, vertexIndex - 1, vertexCount);
                int v2 = getIndex(curveIndex, vertexIndex, vertexCount);
                int v3 = getIndex(curveIndex - 1, vertexIndex, vertexCount);

                // Add two triangles (quad):
                builder.AddTriangle(v0, v1, v2);
                builder.AddTriangle(v0, v2, v3);
            }
        }

        ReplaceMesh(builder.CreateMesh(), modifySharedMesh);
    }
}
