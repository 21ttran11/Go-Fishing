using Niantic.Lightship.AR.Semantics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class SemanticQuerying : MonoBehaviour
{
    [Header("AR Components")]
    public ARCameraManager _cameraMan;
    public ARSemanticSegmentationManager _semanticMan;
    public Camera _arCamera;

    [Header("UI Display")]
    public TMP_Text castingText;
    public GameObject noWater;
    //public TMP_Text _text;

    [Header("Spawning Settings")]
    public GameObject prefabToSpawn;
    public float spawnScale = 0.8f;
    public float minSpawnDistance = 1.0f;
    public float maxSpawnDistance = 2.5f;

    [Header("Ground Detection Settings")]
    public float minHoldTime = 1.0f;   
    public float maxHoldTime = 3.0f;

    private bool hasSpawned = false;
    private bool detectingGround = false;
    private float groundHoldTimer = 0.0f;
    private float requiredHoldTime = 0.0f;

    private string _channel = "ground";
    private float _timer = 0.0f;

    public string Channel => _channel;

    void OnEnable()
    {
        if (_cameraMan != null)
        {
            _cameraMan.frameReceived += OnCameraFrameUpdate;
        }
    }

    void OnDisable()
    {
        if (_cameraMan != null)
        {
            _cameraMan.frameReceived -= OnCameraFrameUpdate;
        }
    }

    private void OnCameraFrameUpdate(ARCameraFrameEventArgs args)
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }
    }

    void Update()
    {
        if (!_semanticMan.subsystem.running || hasSpawned || prefabToSpawn == null)
        {
            return;
        }

        Vector2 centerPos = new Vector2(Screen.width / 2, Screen.height / 2);

        _timer += Time.deltaTime;
        if (_timer > 0.05f)
        {
            var list = _semanticMan.GetChannelNamesAt((int)centerPos.x, (int)centerPos.y);

            if (list.Count > 0)
            {
                _channel = list[0];
                Debug.Log($"Detected semantic channel: {_channel}");
                //_text.text = $"Detected Channel: {_channel}";

                if (_channel == "water")
                {
                    noWater.SetActive(false);
                    if (!detectingGround)
                    {
                        noWater.SetActive(false);
                        detectingGround = true;
                        groundHoldTimer = 0.0f;
                        requiredHoldTime = Random.Range(minHoldTime, maxHoldTime);
                        Debug.Log($"Ground detected! Need to hold for {requiredHoldTime:F2} seconds.");
                    }
                    else
                    {
                        groundHoldTimer += _timer;
                        if (groundHoldTimer >= requiredHoldTime)
                        {
                            noWater.SetActive(false);
                            SpawnInFrontOfCamera();
                            castingText.text = "click to reel";
                        }
                    }
                }
                else
                {
                    noWater.SetActive(true);
                    detectingGround = false;
                    groundHoldTimer = 0.0f;
                }
            }
            else
            {
                Debug.Log("No semantic channel detected at the center.");
                //_text.text = "No channel detected";
                noWater.SetActive(true);
                detectingGround = false;
                groundHoldTimer = 0.0f;
            }

            _timer = 0.0f;
        }
    }

    private void SpawnInFrontOfCamera()
    {
        if (_arCamera == null)
        {
            Debug.LogError("AR Camera reference missing!");
            return;
        }

        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 spawnPos = _arCamera.transform.position + _arCamera.transform.forward * randomDistance;
        GameObject spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        spawned.transform.localScale *= spawnScale;

        hasSpawned = true;

        Debug.Log($"Spawned prefab {randomDistance:F2} meters in front of camera!");
    }
}