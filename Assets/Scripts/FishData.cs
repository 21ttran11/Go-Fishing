using UnityEngine;

[CreateAssetMenu(fileName = "NewFishData", menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishId;         
    public string fishName;       
    public Sprite fishSprite;     
    public string description;    
    public float rarity;          
}