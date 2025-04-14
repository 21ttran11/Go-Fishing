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

    public float targetMoveSpeed = 2f;
    public float targetMoveRange = 400f;
    private float timer = 0f;

    void Start()
    {
        barPosition = slidingBar.anchoredPosition;
    }

    void Update()
    {
        float tilt = -Input.acceleration.y;

        currentSpeed = Mathf.Clamp(tilt * sensitivity, -maxSpeed, maxSpeed);

        barPosition.y += currentSpeed * Time.deltaTime;
        barPosition.y = Mathf.Clamp(barPosition.y, minY, maxY);
        slidingBar.anchoredPosition = barPosition;

        MoveTarget();
        
    }

    private void MoveTarget()
    {
        timer += Time.deltaTime * targetMoveSpeed;
        Vector2 targetPos = target.anchoredPosition;
        targetPos.y = Mathf.Sin(timer) * targetMoveRange;
        target.anchoredPosition = targetPos;
    }

    private void CheckCondition()
    {

    }

    private IEnumerator CountDown()
    {
        yield return null;
    }

}
