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
    public Sprite questionMarkSprite;

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
            Image fishImage = fishUI.GetComponentInChildren<Image>();
            Text fishName = fishUI.GetComponentInChildren<Text>();

            if (caughtFishIds.Contains(fish.fishId))
            {
                fishImage.sprite = fish.fishSprite;
                fishName.text = fish.fishName;
            }
            else
            {
                fishImage.sprite = questionMarkSprite;
                fishName.text = "???";
            }
        }
    }
}