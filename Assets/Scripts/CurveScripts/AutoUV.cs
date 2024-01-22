// Version 2023
//  (Updates: no getters in loops)

using UnityEditor;
using UnityEngine;

public class AutoUV : MonoBehaviour
{
	public Vector2 textureScaleFactor = new Vector2(1, 1);
	public bool UseWorldCoordinates;
	public bool AutoUpdate;
	public bool RecalculateTangents = true;

    void Update()
    {
        if ((transform.hasChanged && UseWorldCoordinates && AutoUpdate) || Input.GetKeyDown(KeyCode.F2)) {
			UpdateUvs();
			transform.hasChanged=false;
		}
    }

	public void UpdateUVs(Mesh mesh) {
		Debug.Log("Updating UVs");

		Vector2[] uv = mesh.uv;
		int[] tris = mesh.triangles;
		Vector3[] verts = mesh.vertices;
		for (int i = 0; i<tris.Length; i+=3) {
			int i1 = tris[i];
			int i2 = tris[i+1];
			int i3 = tris[i+2];
			Vector3 v1 = verts[i1];
			Vector3 v2 = verts[i2];
			Vector3 v3 = verts[i3];
			if (UseWorldCoordinates) {
				v1 = transform.TransformPoint(v1);
				v2 = transform.TransformPoint(v2);
				v3 = transform.TransformPoint(v3);
			}
			Vector3 tangent;
			Vector3 biTangent;
			// TODO: Take vertices that are part of multiple triangles + slight mesh warping into account.
			//  Possible solution:
			//   Store the computed tangent & bitangent for each vertex.
			//   If those have already been computed for at least one of the triangle vertices, assign those to all triangle vertices, instead of recomputing.
			ComputeTangents(v1, v2, v3, out tangent, out biTangent);
			ComputeTriangleUVs(v1, v2, v3, ref uv[i1], ref uv[i2], ref uv[i3], tangent, biTangent);
		}
		mesh.uv=uv;
		if (RecalculateTangents) {
			mesh.RecalculateTangents();
		}
	}

	public void UpdateUvs() {
		// Clone the shared mesh manually, to prevent the "leaking meshes" error:
		Mesh origMesh = GetComponent<MeshFilter>().sharedMesh; 
		Mesh mesh = (Mesh)Instantiate(origMesh);

		UpdateUVs(mesh);

		GetComponent<MeshFilter>().mesh=mesh;
	}

	void ComputeTangents(Vector3 v1, Vector3 v2, Vector3 v3, out Vector3 tangent, out Vector3 biTangent) {
		// TODO: first compute a correct normal for the triangle, using cross product
		//  (see slides, or 3d math)
		Vector3 normal = Vector3.forward;

		// If the triangle has almost zero area, the normal will be small as well, but then the uvs won't matter much anyway:
		if (normal.magnitude<=0.000001) {
			tangent=new Vector3();
			biTangent=new Vector3();
			return;
		}
		normal.Normalize();

		//  Next: we compute a tangent that is perpendicular to both the up vector and the normal,
		// using a cross product:
		tangent = Vector3.Cross(Vector3.up, normal).normalized;

		// TODO: compute a bitangent that is perpendicular to both the normal and the tangent:
		biTangent = Vector3.up;
	}

	void ComputeTriangleUVs(Vector3 v1, Vector3 v2, Vector3 v3, ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, Vector3 tangent, Vector3 biTangent) {
		// Use the dot product onto unit vectors (the tangent and bitangent) for scalar projection. 
		// This gives the coordinates of each of the three points v1, v2 and v3, relative to the vector basis given by tangent & bitangent.
		// (See the 3D Math course for more details!)
		// Those coordinates will be used as the uvs.
		uv1 = new Vector2(Vector3.Dot(v1, tangent), Vector3.Dot(v1, biTangent)) / textureScaleFactor;
		uv2 = new Vector2(Vector3.Dot(v2, tangent), Vector3.Dot(v2, biTangent)) / textureScaleFactor;
		uv3 = new Vector2(Vector3.Dot(v3, tangent), Vector3.Dot(v3, biTangent)) / textureScaleFactor;
	}
}
