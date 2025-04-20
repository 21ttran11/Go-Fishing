using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestCollection : MonoBehaviour
{
    public FishDatabase fishDatabase;
    public GameObject fishItemPrefab;
    public Transform fishGridParent;
    public Sprite questionMarkTag;
    public Sprite newTagSprite;
    public Sprite rarityTag;
    public FishPopup fishPopup;

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
            Image rarityTag = fishUI.transform.Find("Tags/RarityTag").GetComponent<Image>();
            Image newTag = fishUI.transform.Find("Tags/NewTag").GetComponent<Image>();
            Button button = fishUI.GetComponent<Button>();

            if (caughtFishIds.Contains(fish.fishId))
            {
                if (button != null)
                {
                    FishData capturedFish = fish;
                    button.onClick.AddListener(() => {
                        fishPopup.Show(capturedFish, fishItemPrefab);
                    });
                }

                fishImage.sprite = fish.fishSprite;
                fishImage.SetNativeSize();

                nameTag.sprite = fish.nameTag;
                nameTag.SetNativeSize();

                rarityTag.sprite = fish.rarityTag;
                rarityTag.SetNativeSize();

                newTag.sprite = newTagSprite;
            }
            else
            {
                fishImage.sprite = fish.unknownSprite;
                fishImage.SetNativeSize();

                nameTag.sprite = questionMarkTag;
                nameTag.SetNativeSize();

                rarityTag.sprite = null;
                newTag.sprite = null;

                Color transparent = new Color(1, 1, 1, 0);
                rarityTag.color = transparent;
                newTag.color = transparent;
            }
        }
    }
}