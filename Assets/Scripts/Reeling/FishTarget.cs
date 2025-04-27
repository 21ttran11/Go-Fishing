using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTarget : MonoBehaviour
{
    private RectTransform target;

    [Header("Movement Settings")]
    public float targetMoveRange = 900f;
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    public float speedChangeInterval = 3f;
    public float speedLerpTime = 1f;

    private float currentSpeed;
    private float targetSpeed;
    private float speedLerpTimer = 0f;
    private float timer = 0f;
    private float timeSinceLastChange = 0f;

    void Start()
    {
        target = GetComponent<RectTransform>();
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        targetSpeed = currentSpeed;
    }

    void Update()
    {
        timer += Time.deltaTime * currentSpeed;

        Vector2 pos = target.anchoredPosition;
        pos.y = Mathf.Sin(timer) * targetMoveRange;
        target.anchoredPosition = pos;

        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= speedChangeInterval)
        {
            targetSpeed = Random.Range(minSpeed, maxSpeed);
            speedLerpTimer = 0f;
            timeSinceLastChange = 0f;
        }


        if (currentSpeed != targetSpeed)
        {
            speedLerpTimer += Time.deltaTime / speedLerpTime;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedLerpTimer);
        }
    }

    public void StopTarget()
    {
        Destroy(this.gameObject);
    }
}
