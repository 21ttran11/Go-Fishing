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

    public AudioClip sfx;

    public AudioManager audioManager;

    private FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        _ = LoadAndDisplayFish(userId);
    }

    private async Task LoadAndDisplayFish(string userId)
    {
        Dictionary<string, int> fishCatchCounts = await GetCaughtFish(userId);
        DisplayFish(fishDatabase.allFish, fishCatchCounts);
    }

    private async Task<Dictionary<string, int>> GetCaughtFish(string userId)
    {
        var db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        Dictionary<string, int> catchCounts = new Dictionary<string, int>();

        if (snapshot.Exists && snapshot.TryGetValue("caughtFishCount", out Dictionary<string, object> caughtFishCount))
        {
            foreach (var entry in caughtFishCount)
            {
                if (entry.Value is long longValue)
                {
                    catchCounts[entry.Key] = (int)longValue;
                }
                else if (entry.Value is int intValue)
                {
                    catchCounts[entry.Key] = intValue;
                }
                else
                {
                    Debug.LogWarning($"Unexpected value type for {entry.Key}: {entry.Value}");
                }
            }
        }

        return catchCounts;
    }

    private void DisplayFish(List<FishData> allFish, Dictionary<string, int> catchCounts)
    {
        foreach (FishData fish in allFish)
        {
            GameObject fishUI = Instantiate(fishItemPrefab, fishGridParent);
            Image fishImage = fishUI.transform.Find("FishIcon").GetComponent<Image>();
            Image nameTag = fishUI.transform.Find("NameTag").GetComponent<Image>();
            Image rarityTag = fishUI.transform.Find("Tags/RarityTag").GetComponent<Image>();
            Image newTag = fishUI.transform.Find("Tags/NewTag").GetComponent<Image>();
            Button button = fishUI.GetComponent<Button>();

            if (catchCounts.ContainsKey(fish.fishId))
            {
                int count = catchCounts[fish.fishId];
                Debug.Log(count);

                if (button != null)
                {
                    FishData capturedFish = fish;
                    int capturedCount = count;
                    Debug.Log(capturedCount);

                    button.onClick.AddListener(() => {
                        fishPopup.Show(capturedFish, fishItemPrefab, capturedCount);
                    });
                    button.onClick.AddListener(() => {
                        audioManager.PlayAudio(sfx);
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