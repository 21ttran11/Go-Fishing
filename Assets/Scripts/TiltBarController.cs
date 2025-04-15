using System.Collections;
using UnityEngine;

public class TiltBarController : MonoBehaviour
{
    public RectTransform slidingBar;
    public RectTransform target;
    public float sensitivity = 100f;
    public float maxSpeed = 500f;
    public float minY = -500f;
    public float maxY = 500f;

    private float currentSpeed = 0f;
    private Vector2 barPosition;

    void Start()
    {
        barPosition = slidingBar.anchoredPosition;
        target = GameObject.FindWithTag("FishTarget").GetComponent<RectTransform>();
    }

    void Update()
    {
        float tilt = -Input.acceleration.y;

        currentSpeed = Mathf.Clamp(tilt * sensitivity, -maxSpeed, maxSpeed);

        barPosition.y += currentSpeed * Time.deltaTime;
        barPosition.y = Mathf.Clamp(barPosition.y, minY, maxY);
        slidingBar.anchoredPosition = barPosition;
        CheckCondition();

    }

    private void CheckCondition()
    {
        
    }

    private IEnumerator CountDown()
    {
        yield return null;
    }

}
