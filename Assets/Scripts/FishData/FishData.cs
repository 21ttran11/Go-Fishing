using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFishData", menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishId;
    public string fishName;
    public Sprite fishSprite;
    public Sprite nameTag;
    public Sprite unknownSprite;
    public string description;
    public List<FishingZone> availableZones;
    public float rarity;
    public Sprite rarityTag;

    [TextArea(2, 5)]
    public List<string> funFacts = new List<string>();
}