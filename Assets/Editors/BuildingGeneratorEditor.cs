using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingGenerator))]
public class BuildingGeneratorEditor : Editor
{
    private BuildingGenerator generator;

    private void OnEnable()
    {
        generator = (BuildingGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            Undo.RecordObject(generator, "Generated a building");
            GameObject obj = generator.CreateBuilding();
            generator.buildings.Add(obj);
            if (generator.generateAsChild)
                obj.transform.parent = generator.transform;
            
            Undo.RegisterCreatedObjectUndo(obj, "Created building");
            EditorUtility.SetDirty(generator);
        }
    }
}
