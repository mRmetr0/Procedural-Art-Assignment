using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(CityShape))]
public class CityShapeEditor : Editor
{
    private CityShape shape;
    
    private void OnEnable()
    {
        shape = (CityShape)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset"))
        {
            Undo.RecordObject(shape, "Generated a building");
            GameObject obj = shape.Reset();
            obj.transform.parent = shape.transform;
            
            Undo.RegisterCreatedObjectUndo(obj, "Created building");
            EditorUtility.SetDirty(shape);
        }
        GUILayout.Label("T for road. \nY for curb.\nU for lamp. \nI for random prop. ");
    }

    public void OnSceneGUI()
    {
        Resize();
        DrawRoad();
    }

    private void DrawRoad()
    {
        Event e = Event.current;
        GameObject obj = null;
        if (e.keyCode == KeyCode.T)
        {
            Transform handleTransform = shape.transform;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                obj = shape.DrawTile(hit.collider.gameObject, CityShape.TileType.Road);
            }
				
            e.Use();
        }
        
        if (e.keyCode == KeyCode.Y)
        {
            Transform handleTransform = shape.transform;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                obj = shape.DrawTile(hit.collider.gameObject, CityShape.TileType.Curb);
            }
				
            e.Use();
        }
        
        if (e.keyCode == KeyCode.U && e.type == EventType.KeyDown)
        {
            Transform handleTransform = shape.transform;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                obj = shape.DrawTile(hit.collider.gameObject, CityShape.TileType.Lamp);
            }
				
            e.Use();
        }
        
        if (e.keyCode == KeyCode.I && e.type == EventType.KeyDown)
        {
            Transform handleTransform = shape.transform;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                obj = shape.DrawTile(hit.collider.gameObject, CityShape.TileType.Prop);
            }
				
            e.Use();
        }

        if (obj != null)
        {
            EditorUtility.SetDirty(shape);
        }
    }

    private void Resize()
    {
        Vector3 size = shape.transform.localScale;
        var transform = shape.transform;
        
        EditorGUI.BeginChangeCheck();
        size = Handles.ScaleHandle(size, transform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(shape, "Update scale");
            transform.localScale = size;
            
            Undo.RecordObject(shape, "Generated a building");
            GameObject obj = shape.Reset();
            obj.transform.parent = shape.transform;
            
            Undo.RegisterCreatedObjectUndo(obj, "Created building");
            EditorUtility.SetDirty(shape);
        }
    }
}
