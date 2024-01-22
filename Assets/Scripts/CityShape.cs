using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityShape : MonoBehaviour
{
    [SerializeField] private List<GameObject> props;
    [SerializeField] private GameObject curb;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject lamp;
    [SerializeField] private int circleRadius = 0;

    public GameObject CityPlane = null;
    
    public enum TileType
    {
        Curb,
        Road,
        Lamp,
        Prop,
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        var transform1 = this.transform;
        Gizmos.DrawWireCube(transform1.position + new Vector3(transform1.localScale.x/2, 0, transform1.localScale.z/2), transform1.localScale);
    }

    public GameObject Reset()
    {
        DeletePlane();
        
        var transform1 = this.transform;
        int width = (int)transform1.localScale.x;
        int depth = (int)transform1.localScale.z;

        GameObject shape = new GameObject("CityPlane");

        Vector2 center = new Vector2((float)width / 2, (float)depth / 2);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                Vector2 tilePos = new Vector2(i + 0.5f, j + 0.5f);
                if (circleRadius <= 0 || (center - tilePos).magnitude < circleRadius)
                    GeneratePrefab(i, j, shape.transform);
            }
        }

        CityPlane = shape;

        return shape;
    }

    private GameObject GeneratePrefab(int x, int y, Transform parent)
    {
        Vector3 offset = new Vector3(0.5f, 0, 0.5f);
        GameObject tile = curb;
        Instantiate(tile, transform.position+  offset + new Vector3(x, 0, y), Quaternion.identity, parent);
        return tile;
    }

    private void DeletePlane()
    {
        if (CityPlane == null) return;
        DestroyImmediate(CityPlane.gameObject);
    }

    public GameObject DrawTile(GameObject obj, TileType type)
    {
        GameObject tile = null;
        switch (type)
        {
            case TileType.Curb:
                tile = curb;
                break;
            case TileType.Road:
                tile = road;
                break;
            case TileType.Lamp:
                tile = lamp;
                break;
            case TileType.Prop:
                tile = props[Random.Range(0, props.Count)];
                break;
        }

        GameObject newObj = Instantiate(tile, obj.transform.position, obj.transform.rotation, obj.transform.parent);
        DestroyImmediate(obj);
        return newObj;
    }
}
