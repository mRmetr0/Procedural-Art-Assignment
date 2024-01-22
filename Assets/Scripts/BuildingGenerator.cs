using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] wallComponents;
    [SerializeField] private int width, depth;
    [SerializeField] private int minHeight, maxHeight;

    [Space(5)] [Header("Roof Components")] 
    [SerializeField] private GameObject roofCorner;
    [SerializeField] private GameObject roofWall;
    [SerializeField] private GameObject roofMiddle;
    
    [Space(5)]
    [SerializeField] private int generationSeed;
    [SerializeField] private bool randomSeed;
    [SerializeField] public bool generateAsChild;
    [SerializeField] private bool GenerateOnStart = false;
    
    public List<GameObject> buildings = new List<GameObject>();
    

    private void Start()
    {
        if (GenerateOnStart) Instantiate(CreateBuilding());
    }

    public GameObject CreateBuilding()
    {
        DeleteOldBuilding();
        
        if (randomSeed)
            generationSeed = Random.Range(1, 1000);
        System.Random rand = new System.Random(generationSeed);

        GameObject building = new GameObject("Building");
        float componentHeight = wallComponents[0].transform.localScale.y;


        //per floor:
        float buildingHeight = rand.Next(minHeight, maxHeight);
        for (int i = 0; i < buildingHeight; i++) {
            //front
            for (int j = 0; j < width; j++)
            {
                GameObject wall = wallComponents[rand.Next(0, wallComponents.Length)];
                float distFactor = (j - ((float)width / 2) + 0.5f);
                Instantiate(wall, this.transform.position + new Vector3(distFactor, i, (float)-depth / 2 + 0.5f),
                    Quaternion.Euler(0, 0, 0), building.transform);
            }
            //back:
            for (int j = 0; j < width; j++)
            {
                GameObject wall = wallComponents[rand.Next(0, wallComponents.Length)];
                float distFactor = (j - ((float)width / 2) + 0.5f);
                Instantiate(wall, this.transform.position + new Vector3(distFactor, i, (float)depth / 2 - 0.5f),
                    Quaternion.Euler(0, 180, 0), building.transform);
            }
            //left
            for (int j = 0; j < depth; j++)
            {
                GameObject wall = wallComponents[rand.Next(0, wallComponents.Length)];
                float distFactor = j - ((float)depth / 2) + 0.5f;
                Instantiate(wall, this.transform.position + new Vector3(((float)-width / 2)+0.5f, i, distFactor),
                    Quaternion.Euler(0, 90, 0), building.transform);
            }
            //right
            for (int j = 0; j < depth; j++)
            {
                GameObject wall = wallComponents[rand.Next(0, wallComponents.Length)];
                float distFactor = (j - ((float)depth / 2) + 0.5f);
                Instantiate(wall, this.transform.position + new Vector3(((float)width / 2)-0.5f, i, distFactor),
                    Quaternion.Euler(0, -90, 0), building.transform);
            }
        }
        //roof: TODO
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                GameObject roof = null;
                float angle = 0;
                float widthFactor = i - (float)width / 2 + 0.5f;
                float depthFactor = j - (float)depth / 2 + 0.5f;
                
                if ((i == 0 || i == width - 1) && (j == 0 || j == depth - 1))
                {
                    roof = roofCorner;
                    bool pastX = widthFactor > 0;
                    bool pastZ = depthFactor > 0;
                    if (pastX && !pastZ) angle = 90;
                    if (!pastX && !pastZ) angle = 180;
                    if (!pastX && pastZ) angle = -90;
                }
                else if (i == 0 || i == width - 1 || j == 0 || j == depth - 1)
                {
                    roof = roofWall;
                    if (j == depth -1) angle = 180;
                    if (i == 0) angle = 90;
                    if (i == width-1) angle = -90;
                }
                else roof = roofMiddle;
                Instantiate(roof, this.transform.position + new Vector3(widthFactor, buildingHeight, depthFactor), Quaternion.Euler(0, angle, 0),
                    building.transform);
            }
        }

        return building;
    }

    private void DeleteOldBuilding()
    {
        if (buildings.Count == 0)
            return;
        for (int i = buildings.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(buildings[i]);
        }
        buildings.Clear();
    }
}
