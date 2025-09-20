using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;

public class FireSimulation : MonoBehaviour
{
    [Header("Settings")]
    public GameObject firePrefab;        
    public double2 fireStartLatLon;      
    public float spreadSpeed = 0.0005f;  
    public float boundarySlowFactor = 0.3f; 
    public float spreadInterval = 30f;
    public bool slow = false;

    private List<ArcGISPoint> activeFirePositions = new List<ArcGISPoint>();

    void Start()
    {
        ArcGISPoint startPoint = new ArcGISPoint(fireStartLatLon.x, fireStartLatLon.y, 0, ArcGISSpatialReference.WGS84());
        activeFirePositions.Add(startPoint);

        // Spawn initial fire prefab
        GameObject newFire = Instantiate(firePrefab, transform);
        newFire.GetComponent<ArcGISLocationComponent>().Position = startPoint;
        newFire.GetComponent<ArcGISLocationComponent>().Rotation = new ArcGISRotation(0, 90, 0);
        StartCoroutine(FireSpreadLoop());
    }

    IEnumerator FireSpreadLoop()
    {
        while (activeFirePositions.Count > 0)
        {
            yield return new WaitForSeconds(spreadInterval);

            List<ArcGISPoint> newPositions = new List<ArcGISPoint>();

            foreach (ArcGISPoint firePos in activeFirePositions)
            {
                // Generate a few new points around this fire position
                for (int i = 0; i < 4; i++)
                {
                    // Random spread direction in lat/lon space
                    float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
                    float dx = Mathf.Cos(angle) * spreadSpeed;
                    float dy = Mathf.Sin(angle) * spreadSpeed;

                    // Convert Unity X/Z back to lat/lon
                    double newLon = firePos.X + dx;
                    double newLat = firePos.Y + dy;

                    // Slow fire if inside any district boundary
                    if (slow)
                    {
                        dx *= boundarySlowFactor;
                        dy *= boundarySlowFactor;
                        newLon = firePos.X + dx;
                        newLat = firePos.Y + dy;
                        spreadInterval += 20f;
                    }

                    ArcGISPoint newPoint = new ArcGISPoint(newLon, newLat, 0, ArcGISSpatialReference.WGS84());

                    // Spawn fire prefab at new position
                    GameObject newFire = Instantiate(firePrefab, transform);
                    newFire.GetComponent<ArcGISLocationComponent>().Position = newPoint;
                    newFire.GetComponent<ArcGISLocationComponent>().Rotation = new ArcGISRotation(0, 90, 0);

                    newPositions.Add(newPoint);
                }
            }

            activeFirePositions = newPositions;
        }
    }
    public void IncreaseSpeed()
    {
        if(spreadInterval >= 10)
        {
            spreadInterval -= 5;
        }
        StopCoroutine(FireSpreadLoop());
        StartCoroutine(FireSpreadLoop());
    }
    public void DecreaseSpeed()
    {
        if(spreadInterval <= 55)
        {
            spreadInterval += 5;
        }
    }
}
