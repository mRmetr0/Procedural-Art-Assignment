using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public abstract class MeshCreator : MonoBehaviour
{
    public abstract void RecalculateMesh();
    
    protected void ReplaceMesh(Mesh newMesh, bool changeSharedMesh = false) {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (!changeSharedMesh) {
            meshFilter.sharedMesh = newMesh;
        } else {
            meshFilter.sharedMesh.Clear();
            meshFilter.sharedMesh.vertices = newMesh.vertices;
            meshFilter.sharedMesh.normals = newMesh.normals;
            meshFilter.sharedMesh.uv = newMesh.uv;
            meshFilter.sharedMesh.triangles = newMesh.triangles;
            meshFilter.sharedMesh.tangents = newMesh.tangents;
        }
    }
}
