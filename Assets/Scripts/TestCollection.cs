using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestCollection : MonoBehaviour
{
    public FishDatabase fishDatabase;
    public GameObject fishItemPrefab;
    public Transform fishGridParent;
    public Sprite questionMarkTag;

    [Tooltip("List of fish IDs you want to show as caught in test mode.")]
    public List<string> testCaughtFishIds;

    void Start()
    {
        HashSet<string> caughtFishIds = new HashSet<string>(testCaughtFishIds);
        DisplayFish(fishDatabase.allFish, caughtFishIds);
    }

    private void DisplayFish(List<FishData> allFish, HashSet<string> caughtFishIds)
    {
        foreach (FishData fish in allFish)
        {
            GameObject fishUI = Instantiate(fishItemPrefab, fishGridParent);
            Image fishImage = fishUI.transform.Find("FishIcon").GetComponent<Image>();
            Image nameTag = fishUI.transform.Find("NameTag").GetComponent<Image>();

            if (caughtFishIds.Contains(fish.fishId))
            {
                fishImage.sprite = fish.fishSprite;
                fishImage.SetNativeSize();

                nameTag.sprite = fish.nameTag;
                nameTag.SetNativeSize();
            }
            else
            {
                fishImage.sprite = fish.unknownSprite;
                fishImage.SetNativeSize();

                nameTag.sprite = questionMarkTag;
                nameTag.SetNativeSize();
            }
        }
    }
}