using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TiltBarController : MonoBehaviour
{
    public RectTransform slidingBar;
    public RectTransform target;
    public float sensitivity = 10000f;
    public float maxSpeed = 900f;
    public float minY = -500f;
    public float maxY = 500f;

    public float completion = 0.5f;
    public float fillSpeed = 0.2f;
    public float drainSpeed = 0.1f;

    private float currentSpeed = 0f;
    private Vector2 barPosition;
    private bool isOverlapping = false;
    private bool isDraining = false;
    private bool hasWon = false;

    public UnityEngine.UI.Image completionBarUI;
    public UnityEvent onWin;
    public UnityEvent onComplete;
    public GameObject lostUI;

    void Start()
    {
        barPosition = slidingBar.anchoredPosition;
        target = GameObject.FindWithTag("FishTarget").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (hasWon) return; // Prevent further updates after win

        float tilt = Input.acceleration.y;
        currentSpeed = Mathf.Clamp(tilt * Mathf.Abs(tilt) * sensitivity, -maxSpeed, maxSpeed);

        barPosition.y += currentSpeed * Time.deltaTime;
        barPosition.y = Mathf.Clamp(barPosition.y, minY, maxY);
        slidingBar.anchoredPosition = barPosition;

        CheckCondition();

        if (isOverlapping)
        {
            completion += fillSpeed * Time.deltaTime;
            completion = Mathf.Clamp01(completion);
        }
        else if (!isDraining)
        {
            StartCoroutine(DrainAfterDelay(1f));
        }

        if (completion >= 1f && !hasWon)
        {
            hasWon = true;
            Debug.Log("You win!");
            onWin?.Invoke();
            onComplete?.Invoke();
        }
        else if (completion <= 0f)
        {
            Debug.Log("Game Over! You lost.");
            lostUI.SetActive(true);
            onComplete?.Invoke();
        }

        completionBarUI.fillAmount = completion;
    }

    private void CheckCondition()
    {
        isOverlapping = RectOverlaps(slidingBar, target);
    }

    private bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Vector3[] aCorners = new Vector3[4];
        Vector3[] bCorners = new Vector3[4];
        a.GetWorldCorners(aCorners);
        b.GetWorldCorners(bCorners);

        Rect aRect = new Rect(aCorners[0], aCorners[2] - aCorners[0]);
        Rect bRect = new Rect(bCorners[0], bCorners[2] - bCorners[0]);

        return aRect.Overlaps(bRect);
    }

    private IEnumerator DrainAfterDelay(float delay)
    {
        isDraining = true;
        yield return new WaitForSeconds(delay);

        while (!isOverlapping && completion > 0f)
        {
            completion -= drainSpeed * Time.deltaTime;
            completion = Mathf.Clamp01(completion);
            yield return null; 
        }

        isDraining = false;
    }
}