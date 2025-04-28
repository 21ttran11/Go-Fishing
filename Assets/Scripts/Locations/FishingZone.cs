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
        float radiusInMeters = radiusKm * 1000f;

        float distance = Vector2.Distance(playerLocation, coordinates);
        return distance <= radiusInMeters; 
    }
}