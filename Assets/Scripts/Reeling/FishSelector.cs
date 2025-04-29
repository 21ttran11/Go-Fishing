using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UI;

public class FishSelector : MonoBehaviour
{
    public FishDatabase fishDatabase;
    public GameObject fishItemPrefab;
    public Canvas canvas;
    public Sprite newTagSprite;
    public GameObject popUp;
    public GameObject homeButton;
    public SceneChanger sceneChanger;

    public GameObject winningBackground;

    [SerializeField]
    public List<FishData> availableFish = new List<FishData>();
    private LocationService locationService;

    private FishData selectedFish;
    private FirebaseFirestore db;

    public FishingZoneDatabase fishingZones;  

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        locationService = FindObjectOfType<LocationService>();
    }

    public void GetRandomFish()
    {

        if (fishDatabase == null || fishDatabase.allFish == null || fishDatabase.allFish.Count == 0)
        {
            Debug.LogWarning("Fish database is missing or empty!");
            return;
        }

        List<FishData> availableFish = FilterFishByLocation(fishDatabase.allFish);

        if (availableFish.Count == 0)
        {
            Debug.LogWarning("No fish available in this location!");
            return;
        }

        int index = Random.Range(0, availableFish.Count);
        selectedFish = availableFish[index];
        Debug.Log("Caught a " + selectedFish.fishName);
        CaughtPopup();
    }

    private List<FishData> FilterFishByLocation(List<FishData> allFish)
    {
        Vector2 playerLocation = new Vector2(locationService.latitude, locationService.longitude);

        if (locationService == null)
        {
            Debug.LogError("Location service is not initialized.");
            return new List<FishData>();
        }

        foreach (var fish in allFish)
        {
            bool isFishInAvailableZone = false;

            if (fish.availableZones.Count == 0)
            {
                availableFish.Add(fish);
                continue;
            }

            foreach (var zone in fish.availableZones)
            {
                if (zone.IsWithinZone(playerLocation))
                {
                    isFishInAvailableZone = true;
                    break;  
                }
            }

            if (isFishInAvailableZone)
            {
                availableFish.Add(fish);
            }
        }

        return availableFish;
    }

    public void CaughtPopup()
    {
        if (selectedFish == null)
        {
            Debug.LogWarning("No fish selected!");
            return;
        }

        popUp.SetActive(true);
        homeButton.SetActive(true);
        winningBackground.SetActive(true);
        GameObject fishUI = Instantiate(fishItemPrefab, canvas.transform);

        RectTransform fishRect = fishUI.GetComponent<RectTransform>();
        fishRect.anchorMin = new Vector2(0.5f, 0.5f);
        fishRect.anchorMax = new Vector2(0.5f, 0.5f);
        fishRect.pivot = new Vector2(0.5f, 0.5f);
        fishRect.anchoredPosition = Vector2.zero;
        fishRect.anchoredPosition += new Vector2(0f, 120f);
        fishRect.localScale = Vector3.one * 1.3f;

        Image fishImage = fishUI.transform.Find("FishIcon").GetComponent<Image>();
        Image nameTag = fishUI.transform.Find("NameTag").GetComponent<Image>();
        Image rarityTag = fishUI.transform.Find("Tags/RarityTag").GetComponent<Image>();
        Image newTag = fishUI.transform.Find("Tags/NewTag").GetComponent<Image>();
        Button button = fishUI.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(() => {
                sceneChanger.ChangeScene("Collection");
            });
        }

        fishImage.sprite = selectedFish.fishSprite;
        fishImage.SetNativeSize();

        nameTag.sprite = selectedFish.nameTag;
        nameTag.SetNativeSize();

        rarityTag.sprite = selectedFish.rarityTag;
        rarityTag.SetNativeSize();

        if (newTagSprite != null && newTag != null)
        {
            newTag.sprite = newTagSprite;
            newTag.SetNativeSize();
        }

        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        _ = SaveCaughtFish(userId, selectedFish.fishId);
    }

    private async Task SaveCaughtFish(string userId, string fishId)
    {
        string userIdCheck = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userIdCheck))
        {
            Debug.LogError("User is not signed in!");
            return;
        }
        Debug.Log("User ID: " + userIdCheck);

        DocumentReference docRef = db.Collection("users").Document(userId);

        Debug.Log("Fetching document for user: " + userIdCheck);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Debug.Log("Document exists");
        }
        else
        {
            Debug.Log("Document does not exist, creating new one");
        }

        if (snapshot.Exists)
        {
            Dictionary<string, object> caughtFish = snapshot.GetValue<Dictionary<string, object>>("caughtFish");
            if (caughtFish == null)
            {
                Debug.Log("Creating caughtFish dictionary");
                caughtFish = new Dictionary<string, object>();
            }

            if (!caughtFish.ContainsKey(fishId))
            {
                Debug.Log("Adding fish");
                caughtFish.Add(fishId, true);
            }

            await docRef.UpdateAsync("caughtFish", caughtFish);
        }
        else
        {
            Dictionary<string, object> caughtFish = new Dictionary<string, object>
            {
                { fishId, true }
            };
            await docRef.SetAsync(new Dictionary<string, object> { { "caughtFish", caughtFish } });
        }
    }
}