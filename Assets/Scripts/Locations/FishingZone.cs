using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishingZone
{
    public string zoneName;
    public Vector2 coordinates;
    public float radiusKm = 0.5f;  
    public List<FishData> availableFish; 
}