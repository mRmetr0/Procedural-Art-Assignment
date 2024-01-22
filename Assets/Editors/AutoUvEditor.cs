using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// [CustomEditor(typeof(AutoUV))]
// public class AutoUvEditor : Editor
// {
//     public override void OnInspectorGUI() {
//         AutoUV targetUv = (AutoUV)target;
//
//         if (GUILayout.Button("Recalculate UVs")) {
//             targetUv.UpdateUvs();
//             EditorUtility.SetDirty(targetUv); // otherwise the new mesh won't be saved into the scene!
//         }
//         DrawDefaultInspector();
//     }
// }
