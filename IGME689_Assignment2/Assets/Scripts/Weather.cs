using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Unity.Mathematics;
using TMPro;

public class Weather : MonoBehaviour
{
    public ArcGISMapComponent arcGISMap;
    public FireDistrictData fireManager;

    public GameObject rainPrefab;
    public float spawnHeight = 200f;

    public double latitude = 40.7128;
    public double longitude = -74.0060;

    [Header("OpenWeatherMap")]
    public string apiKey = "2774e392ae40edc491c8b34071231687";

    public TMP_Text location;
    public TMP_Text temperature;
    public TMP_Text pressure;
    public TMP_Text seaLevel;
    public TMP_Text groundLevel;

    public void CallWeatherAPI()
    {
        StopCoroutine(UpdateWeather());
        StartCoroutine(UpdateWeather());
    }

    double3 GeoToUnityPosition(double lat, double lon, float height)
    {
        ArcGISPoint point = new ArcGISPoint(lon, lat, height, ArcGISSpatialReference.WGS84());
        return arcGISMap.View.GeographicToWorld(point);
    }
    IEnumerator UpdateWeather()
    {
        while (true)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";
           // Debug.Log(url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                WeatherData data = JsonUtility.FromJson<WeatherData>(request.downloadHandler.text);
                ShowData(data);
            }
            else
            {
                Debug.LogWarning("Weather API request failed");
            }

            yield return new WaitForSeconds(3f);
        }
    }

    void ShowData(WeatherData data)
    {
        location.text = data.name;
        temperature.text = "Temperature : " + data.main.temp;
        pressure.text = "Pressure : " + data.main.pressure;
        seaLevel.text = "Sea Level : " + data.main.sea_level;
        groundLevel.text = "Ground Level : " + data.main.grnd_level;
    }
    [System.Serializable]
    public class WeatherData
    {
        public WeatherMain main;
        public WindData wind;
        public string name;
    }

    [System.Serializable]
    public class WindData
    {
        public float _1h; 
    }
    [System.Serializable]
    public class WeatherMain
    {
        public float temp;
        public float pressure;
        public float sea_level;
        public float grnd_level;
    }


}
