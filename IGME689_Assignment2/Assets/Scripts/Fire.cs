/*using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Editor.UI;

public class GeoJSONFireDistrictsLoader : MonoBehaviour
{
    [Header("ArcGIS Map View")]
    public ArcGISMapComponent arcGISMap;

    [Header("GeoJSON URL")]
    public string geoJsonUrl = "https://services3.arcgis.com/dkpOfuz3lCHMxq7I/arcgis/rest/services/Fire_Districts/FeatureServer/4/query?outFields=*&where=1=1&f=geojson";

    [Header("Visual Settings")]
    public Color fillColor = new Color(1f, 0.5f, 0f, 0.4f);
    public Color outlineColor = Color.red;
    public float outlineWidth = 1f;

    private ArcGISSpatialFeatureFilterInstanceDataEditor featureTable;
    private ArcGISFeatureLayer featureLayer;

    void Start()
    {
        StartCoroutine(LoadGeoJSON());
    }

    IEnumerator LoadGeoJSON()
    {
        UnityWebRequest www = UnityWebRequest.Get(geoJsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load GeoJSON: " + www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            // Parse GeoJSON
            var geoJson = JSONUtility.FromJson<GeoJSONObject>(jsonText);

            // Create a feature collection table from the GeoJSON
            featureTable = new ArcGISFeatureCollectionTable(geoJson);

            // Create a feature layer
            featureLayer = new ArcGISFeatureLayer(featureTable);

            // Optionally set renderer for styling
            var simpleRenderer = new SimpleRenderer();
            var symbol = new SimpleFillSymbol
            {
                Color = fillColor,
                OutlineColor = outlineColor,
                OutlineWidth = outlineWidth
            };
            simpleRenderer.Symbol = symbol;
            featureLayer.Renderer = simpleRenderer;

            // Add layer to map
            arcGISMap.Map.OperationalLayers.Add(featureLayer);
        }
    }
}
*//*
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;
using json;  // Import the SimpleJSON.cs file into your project

public class GeoJSONSpawner : MonoBehaviour
{
    public string geoJsonUrl = "https://services3.arcgis.com/dkpOfuz3lCHMxq7I/arcgis/rest/services/Fire_Districts/FeatureServer/4/query?outFields=*&where=1=1&f=geojson";
    public GameObject prefab;

    void Start()
    {
        StartCoroutine(LoadGeoJSON());
    }

    IEnumerator LoadGeoJSON()
    {
        UnityWebRequest www = UnityWebRequest.Get(geoJsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load GeoJSON: " + www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            ParseAndSpawn(jsonText);
        }
    }

    void ParseAndSpawn(string jsonText)
    {
        var root = JSON.Parse(jsonText);

        var features = root["features"];
        foreach (JSONNode feature in features)
        {
            var coords = feature["geometry"]["coordinates"];

            // GeoJSON polygon: [[[ [lon, lat], [lon, lat], ... ]]]
            var polygon = coords[0];

            foreach (JSONNode point in polygon)
            {
                float lon = point[0].AsFloat;
                float lat = point[1].AsFloat;

                // Simple conversion: lon -> X, lat -> Z
                Vector3 pos = new Vector3(lon, 0, lat);

                // Optional: normalize by subtracting first point so they're near origin
                if (prefab != null)
                {
                    Instantiate(prefab, pos, Quaternion.identity);
                }
            }
        }
    }
}
*/