using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Curve curve;
    [SerializeField] private int towerAmount;
    private List<GameObject> towers = new List<GameObject>();

    public void PlaceTowers()
    {
        
        if (curve.points.Count == 0) return;
        ClearTowers();
        float towerDist = GetDist();
        for (int i = 0; i < towerAmount; i++)
        {
            GameObject newTower = Instantiate(tower, transform.position, Quaternion.identity, transform);
            newTower.transform.Rotate(Vector3.up, 360/towerAmount * i);
            newTower.transform.localPosition += newTower.transform.forward * towerDist;
            towers.Add(newTower);
        }
    }

    private void ClearTowers()
    {
        if (towers.Count == 0) return;
        for (int i = towers.Count-1; i >= 0; i--)
        {
            DestroyImmediate(towers[i]);
        }
    }

    private float GetDist()
    {
        Bounds bounds = new Bounds(curve.points[0], Vector3.zero);
        foreach (Vector3 pos in curve.points)
        {
            bounds.Encapsulate(pos);
        }

        return bounds.center.x;
    }
}

[CustomEditor(typeof(TowerPlacer))]
public class TowerPlacerScript : Editor
{
    public override void OnInspectorGUI() {
        TowerPlacer placer = (TowerPlacer)target;

        if (GUILayout.Button("Place Towers")) {
            placer.PlaceTowers();
            EditorUtility.SetDirty(placer); // otherwise the new mesh won't be saved into the scene!
        }
        DrawDefaultInspector();
    }
}

