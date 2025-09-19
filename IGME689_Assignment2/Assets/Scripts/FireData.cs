using SimpleJSON; // Make sure this is imported
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Unity.Mathematics;

public class FireData : MonoBehaviour
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

            //Debug.Log(coordinates);
            float baseLon = coordinates[0][0];
            float baseLat = coordinates[0][1];

            //Debug.Log(baseLon); 
            //Debug.Log(baseLat);

            /*            foreach (JSONNode point in coordinates)
                        {
                            float lon = point[0];
                            float lat = point[1];

                            // Simple conversion: lon -> X, lat -> Z
                            Vector3 pos = new Vector3(lon - baseLon, 0, lat - baseLat);
                            //Debug.Log(pos);

                            if (prefab != null)
                                Instantiate(prefab, pos, Quaternion.identity);
                        }*/

            ArcGISPoint point = new ArcGISPoint(baseLon, baseLat, 0, ArcGISSpatialReference.WGS84());
            double3 spawnPoint = arcGISMap.View.GeographicToWorld(point);
            Instantiate(prefab, new Vector3((float)spawnPoint.x, (float)spawnPoint.y, 0), quaternion.identity);
        }
    }
}
