using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishingZone", menuName = "Location/Fishing Zone")]
public class FishingZone : ScriptableObject
{
    public string zoneName;
    public Vector2 coordinates; 
    public float radiusKm = 0.5f; 
    public List<FishData> specialFish;

    public bool IsWithinZone(Vector2 playerLocation)
    {
        float lat1 = Mathf.Deg2Rad * coordinates.x;
        float lon1 = Mathf.Deg2Rad * coordinates.y;
        float lat2 = Mathf.Deg2Rad * playerLocation.x;
        float lon2 = Mathf.Deg2Rad * playerLocation.y;

        float earthRadius = 6371000f;

        float dLat = lat2 - lat1;
        float dLon = lon2 - lon1;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        float distance = earthRadius * c; 

        return distance <= radiusKm * 1000f; 
    }
}