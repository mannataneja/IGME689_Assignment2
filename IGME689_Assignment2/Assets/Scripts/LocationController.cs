using UnityEngine;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;

public class LocationController : MonoBehaviour
{
    [SerializeField] private ArcGISMapComponent arcGISMapComponent;
    [SerializeField] private ArcGISCameraComponent arcGISCameraComponent;
    [SerializeField] private FireDistrictData firemanager;
    [SerializeField] private Weather weatherManager;
    public int currentLocationIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLocationIndex = 0;
    }

    // Update is called once per frame
/*    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoToNextLocation();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
    }*/
    public void GoToNextLocation()
    {
        // Make sure the current location index loops back to the start after the last location has been visited
        currentLocationIndex = (currentLocationIndex + 1) % firemanager.locations.Count;
        SetLocationFromIndex(currentLocationIndex);
    }
    public void GoToPrevioustLocation()
    {
        // Make sure the current location index loops back to the start after the last location has been visited
        currentLocationIndex = (currentLocationIndex - 1) % firemanager.locations.Count;
        SetLocationFromIndex(currentLocationIndex);
    }
    private void SetLocationFromIndex(int index)
    {
        // Update the map origin
        arcGISMapComponent.OriginPosition = new ArcGISPoint(firemanager.locations[index].x, firemanager.locations[index].y, 0, ArcGISSpatialReference.WGS84());
        arcGISMapComponent.UpdateHPRoot();

        // Update the camera position
        ArcGISLocationComponent cameraLocation = arcGISCameraComponent.GetComponent<ArcGISLocationComponent>();
        cameraLocation.Position = new ArcGISPoint(firemanager.locations[index].x, firemanager.locations[index].y, cameraLocation.Position.Z, ArcGISSpatialReference.WGS84());

        weatherManager.longitude = firemanager.locations[index].x;
        weatherManager.latitude = firemanager.locations[index].y;
        weatherManager.CallWeatherAPI();
    }
}
