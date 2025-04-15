using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FishTarget : MonoBehaviour
{
    public RectTransform target;
    public float targetMoveSpeed = 2f;
    public float targetMoveRange = 400f;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * targetMoveSpeed;
        Vector2 targetPos = target.anchoredPosition;
        targetPos.y = Mathf.Sin(timer) * targetMoveRange;
        target.anchoredPosition = targetPos;
    }
}
