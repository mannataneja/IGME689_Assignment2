using SimpleJSON; // Make sure this is imported
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Unity.Mathematics;

public class FireDistrictData : MonoBehaviour
{
    public ArcGISMapComponent arcGISMap;
    public string geoJsonUrl = "https://services3.arcgis.com/dkpOfuz3lCHMxq7I/arcgis/rest/services/Fire_Districts/FeatureServer/4/query?outFields=*&where=1=1&f=geojson";
    public GameObject prefab; // prefab to spawn

    void Start()
    {
        StartCoroutine(FetchAndSpawn());
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

            Debug.Log(coordinates);
            float baseLon = coordinates[0][0];
            float baseLat = coordinates[0][1];

            //Debug.Log(baseLon); 
            //Debug.Log(baseLat);

            foreach (JSONNode perimeterPoint in coordinates)
            {
                float lon = perimeterPoint[0];
                float lat = perimeterPoint[1];
                Debug.Log(lon);

                ArcGISPoint point = new ArcGISPoint(lon, lat, 0, ArcGISSpatialReference.WGS84());
                GameObject newPoint = Instantiate(prefab, transform);
                newPoint.GetComponent<ArcGISLocationComponent>().Position = point;
                newPoint.GetComponent<ArcGISLocationComponent>().Rotation = new ArcGISRotation(0, 90, 0);
            }

            
        }
    }
}