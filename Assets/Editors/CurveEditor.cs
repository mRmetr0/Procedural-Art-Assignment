using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(Curve))]
public class CurveEditor : Editor
{
    private Curve curve;

	private void OnEnable() {
		curve = (Curve)target;
	}

    // DONE (1.1): Add a button to the Curve's inspector 
    public override void OnInspectorGUI()
    {
	    base.OnInspectorGUI();
	    //Exercise 1:
	    if (GUILayout.Button("Apply"))
		    curve.Apply();
    }

    // This method is called by Unity whenever it renders the scene view.
    // We use it to draw gizmos, and deal with changes (dragging objects)
    void OnSceneGUI() {
		if (curve.points == null)
			return;

		DrawAndMoveCurve();

		// Add new points if needed:
		Event e = Event.current;
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space) {
			Debug.Log("Space pressed - trying to add point to curve");
			AddPoint();
			e.Use(); // To prevent the event from being handled by other editor functionality
		}

		ShowAndMovePoints();
    }

	// Example: here's how to draw a handle:
	void DrawAndMoveCurve() {
		Transform handleTransform = curve.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		EditorGUI.BeginChangeCheck();
		Vector3 newPosition = Handles.PositionHandle(handleTransform.position, handleRotation);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(curve.transform, "Move curve");
			EditorUtility.SetDirty(curve);
			curve.transform.position = newPosition;
		}
	}

	// Tries to add a point to the curve, where the mouse is in the scene view.
	// Returns true if a change was made.
	void AddPoint() {
		Transform handleTransform = curve.transform;

		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			Debug.Log("Adding spline point at mouse position: " + hit.point);
			
			Vector3 newPos = Handles.PositionHandle(curve.transform.TransformPoint(hit.point), Quaternion.identity);

			Undo.RecordObject(curve, "Update location");
			curve.points.Add(curve.transform.InverseTransformPoint(hit.point));
			EditorUtility.SetDirty(curve);

		}
	}

	// Show points in scene view, and check if they're changed:
	void ShowAndMovePoints() {
		Vector3 previousPoint = curve.points[curve.points.Count-1];
		for (int i = 0; i < curve.points.Count; i++) {
			Vector3 currentPoint = curve.points[i];

			curve.Apply();

			// TODO (1.2): Draw a line from previous point to current point, in white
			//Handles.DrawLine(previousPoint, currentPoint);
			if (i > 0)
				Handles.DrawLine(previousPoint, curve.transform.TransformPoint(currentPoint));

			previousPoint = currentPoint;

			// TODO (1.2): 
			// Draw position handles (see the above example code)
			// Record in the undo list and mark the scene dirty when the handle is moved.
			EditorGUI.BeginChangeCheck();
			// TODO: correctly go from local to world space and back...
			//previousPoint = Handles.PositionHandle(previousPoint, Quaternion.identity);
			previousPoint = Handles.PositionHandle(curve.transform.TransformPoint(previousPoint), Quaternion.identity);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(curve, "Update location");
				curve.points[i] = curve.transform.InverseTransformPoint(previousPoint);
			}
		}
	}
}
