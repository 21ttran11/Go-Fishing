using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPopup : MonoBehaviour
{
    public Transform fishItemContainer;
    public TMP_Text descriptionText;
    public TMP_Text funfactsText;
    public TMP_Text catchCountText; 
    public Animator animator;
    public GameObject backgroundButton;

    private GameObject currentFishItem;

    public void Show(FishData fishData, GameObject fishItemPrefab, int catchCount) 
    {
        currentFishItem = Instantiate(fishItemPrefab, fishItemContainer);

        Image fishImage = currentFishItem.transform.Find("FishIcon").GetComponent<Image>();
        fishImage.sprite = fishData.fishSprite;
        fishImage.SetNativeSize();

        Image nameTag = currentFishItem.transform.Find("NameTag").GetComponent<Image>();
        nameTag.sprite = fishData.nameTag;
        nameTag.SetNativeSize();

        Image rarityTag = currentFishItem.transform.Find("Tags/RarityTag").GetComponent<Image>();
        rarityTag.sprite = fishData.rarityTag;
        rarityTag.SetNativeSize();

        descriptionText.text = fishData.description;
        funfactsText.text = "";

        foreach (string fact in fishData.funFacts)
        {
            funfactsText.text += "- " + fact + "\n";
        }

        if (catchCountText != null)
        {
            catchCountText.text = catchCount.ToString();
        }

        currentFishItem.transform.localScale = Vector3.one * 1.3f;

        animator.Play("Opening");
        backgroundButton.SetActive(true);
    }

    public void Hide()
    {
        animator.Play("Closing");
        backgroundButton.SetActive(false);
    }

    public void Cleanup()
    {
        descriptionText.text = string.Empty;
        funfactsText.text = string.Empty;

        if (catchCountText != null)
        {
            catchCountText.text = string.Empty;
        }

        if (currentFishItem != null)
        {
            Destroy(currentFishItem);
            currentFishItem = null;
        }
    }
}