using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPopup : MonoBehaviour
{
    public Transform fishItemContainer;
    public TMP_Text descriptionText;
    public TMP_Text funfactsText;
    public Animator animator;
    public GameObject backgroundButton;

    private GameObject currentFishItem;

    public void Show(FishData fishData, GameObject fishItemPrefab)
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

        foreach (string fact in fishData.funFacts)
        {
            funfactsText.text += "- " + fact + "\n";
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

        if (currentFishItem != null)
        {
            Destroy(currentFishItem);
            currentFishItem = null;
        }
    }
}