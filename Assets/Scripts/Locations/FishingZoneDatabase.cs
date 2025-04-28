using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FishingZoneDatabase", menuName = "Location/FishingZoneDatabase")]
public class FishingZoneDatabase : ScriptableObject
{
    public List<FishingZone> allFishingZones; 
}