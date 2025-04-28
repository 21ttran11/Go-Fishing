using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

public class LocationManager : MonoBehaviour
{
    public XROrigin sessionOrigin;
    private ARCameraManager cameraManager;

    public List<FishingZone> fishingZones;

    void Start()
    {
        cameraManager = sessionOrigin.GetComponent<ARCameraManager>();
    }

    public bool IsPlayerInFishingZone(Vector2 playerLocation)
    {
        foreach (var zone in fishingZones)
        {
            if (IsInZone(playerLocation, zone))
            {
                return true;
            }
        }
        return false; 
    }

    private bool IsInZone(Vector2 playerLocation, FishingZone zone)
    {
        float distance = GetDistanceInKm(playerLocation, zone.coordinates);
        return distance <= zone.radiusKm;
    }

    private float GetDistanceInKm(Vector2 coord1, Vector2 coord2)
    {
        float R = 6371f; 
        float dLat = Mathf.Deg2Rad * (coord2.x - coord1.x);
        float dLon = Mathf.Deg2Rad * (coord2.y - coord1.y);
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(Mathf.Deg2Rad * coord1.x) * Mathf.Cos(Mathf.Deg2Rad * coord2.x) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return R * c; 
    }
}