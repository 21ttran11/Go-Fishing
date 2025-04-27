using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FishDatabase", menuName = "Fishing/Fish Database")]
public class FishDatabase : ScriptableObject
{
    public List<FishData> allFish;
}