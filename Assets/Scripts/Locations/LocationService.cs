using UnityEngine;

public class LocationService : MonoBehaviour
{
    public float latitude;
    public float longitude;

    void Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled on this device.");
            return;
        }

        Input.location.Start();
    }

    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            Debug.Log("Latitude: " + latitude + ", Longitude: " + longitude);
        }
        else
        {
            Debug.LogWarning("Location service is not running.");
        }
    }

    void OnApplicationQuit()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Stop();
        }
    }
}