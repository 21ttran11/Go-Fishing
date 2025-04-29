using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UI;

public class FishCollectionDisplay : MonoBehaviour
{
    public FishDatabase fishDatabase;
    public GameObject fishItemPrefab;
    public Transform fishGridParent;
    public Sprite questionMarkTag;
    public Sprite newTagSprite;
    public Sprite rarityTag;
    public FishPopup fishPopup;

    private FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        _ = LoadAndDisplayFish(userId);
    }

    private async Task LoadAndDisplayFish(string userId)
    {
        HashSet<string> caughtFishIds = await GetCaughtFish(userId);
        DisplayFish(fishDatabase.allFish, caughtFishIds);
    }

    private async Task<HashSet<string>> GetCaughtFish(string userId)
    {
        HashSet<string> caughtFishIds = new HashSet<string>();
        DocumentReference docRef = db.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists && snapshot.ContainsField("caughtFish"))
        {
            Dictionary<string, object> caughtFish = snapshot.GetValue<Dictionary<string, object>>("caughtFish");
            foreach (var kvp in caughtFish)
            {
                if ((bool)kvp.Value == true)
                {
                    caughtFishIds.Add(kvp.Key);
                }
            }
        }

        return caughtFishIds;
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

                rarityTag.sprite = fish.rarityTag;
                rarityTag.SetNativeSize();

                newTag.sprite = newTagSprite;

                Color transparent = new Color(1, 1, 1, 0);
                newTag.color = transparent;
            }
        }
    }
}