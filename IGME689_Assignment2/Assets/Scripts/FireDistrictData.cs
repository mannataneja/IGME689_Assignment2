using SimpleJSON; // Make sure this is imported
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Unity.Mathematics;

public class FireDistrictData : MonoBehaviour
{
    public ArcGISMapComponent arcGISMap;
    public ArcGISLocationComponent cameraLocation;
    public string geoJsonUrl = "https://services3.arcgis.com/dkpOfuz3lCHMxq7I/arcgis/rest/services/Fire_Districts/FeatureServer/4/query?outFields=*&where=1=1&f=geojson";
    public GameObject prefab; // prefab to spawn
    public List<double2> locations = new List<double2>();
    void Start()
    {
        StartCoroutine(FetchAndSpawn());
        locations.Add(new double2(cameraLocation.Position.X, cameraLocation.Position.Y));
    }

    IEnumerator FetchAndSpawn()
    {
        UnityWebRequest request = UnityWebRequest.Get(geoJsonUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching GeoJSON: " + request.error);
            yield break;
        }

        JSONNode root = JSON.Parse(request.downloadHandler.text);
        var features = root["features"];

        foreach (JSONNode feature in features)
        {
            var coordinates = feature["geometry"]["coordinates"][0];

            //Debug.Log(coordinates);
            float baseLon = coordinates[0][0];
            float baseLat = coordinates[0][1];
            if(baseLat != 0 || baseLon != 0)
            {
                locations.Add(new double2(baseLon, baseLat));

            }
            //Debug.Log(baseLon); 
            //Debug.Log(baseLat);

            int c = 0;

            float minLon = 0;
            float maxLon = 0;
            float minLat = 0;
            float maxLat = 0;

            foreach (JSONNode perimeterPoint in coordinates)
            {
                c++;
                if(c % 2 == 0)
                {
                    continue;
                }
                float lon = perimeterPoint[0];
                float lat = perimeterPoint[1];

                if (lat == 0 || lon == 0) continue;

                if (Mathf.Abs(lon) < minLon) minLon = lon;
                if (Mathf.Abs(lat) < minLat) minLat = lat;
                if (Mathf.Abs(lon) > maxLon) maxLon = lon;
                if (Mathf.Abs(lat) > maxLat) maxLat = lat;
                
                ArcGISPoint point = new ArcGISPoint(lon, lat, 0, ArcGISSpatialReference.WGS84());
                GameObject newPoint = Instantiate(prefab, transform);
                newPoint.GetComponent<ArcGISLocationComponent>().Position = point;
                newPoint.GetComponent<ArcGISLocationComponent>().Rotation = new ArcGISRotation(0, 90, 0);
            }

            //locations.Add(new double2((minLon + maxLon) / 2, (minLat + maxLat) / 2));
        }
    }
}