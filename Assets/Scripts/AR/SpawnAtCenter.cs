using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;
using Niantic.Lightship.AR.Semantics;

public class SpawnAtCenter : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private float scaler = 1f;

    [Range(0, 100)]
    [SerializeField] private int spawnLikelihood = 33;

    private ARRaycastManager raycastManager;
    private SemanticQuerying semanticQuerying;

    private GameObject spawnedObject;
    private bool hasSpawned = false;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        semanticQuerying = FindObjectOfType<SemanticQuerying>();

        StartCoroutine(TrySpawn());
    }

    IEnumerator TrySpawn()
    {
        float timeout = 10f;
        float elapsed = 0f;

        while (semanticQuerying == null || string.IsNullOrEmpty(semanticQuerying.Channel))
        {
            semanticQuerying = FindObjectOfType<SemanticQuerying>();
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
            if (elapsed > timeout)
            {
                Debug.LogWarning("No semantic data detected in time.");
                yield break;
            }
        }

        yield return new WaitForSeconds(1f); 

        TryRaycastAndSpawn();
    }

    void TryRaycastAndSpawn()
    {
        if (hasSpawned)
            return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            if (semanticQuerying.Channel == "ground")
            {
                int roll = Random.Range(0, 100);
                Debug.Log("Spawn likelihood roll => " + roll);

                if (roll <= spawnLikelihood)
                {
                    GameObject objToSpawn = spawnObjects[Random.Range(0, spawnObjects.Count)];
                    spawnedObject = Instantiate(objToSpawn, hitPose.position, Quaternion.identity);
                    spawnedObject.transform.localScale *= scaler;

                    Debug.Log("Spawned object at center of screen!");
                    hasSpawned = true;
                }
                else
                {
                    Debug.Log("Did not spawn due to random roll.");
                }
            }
            else
            {
                Debug.Log("Center screen is not ground, semantic channel: " + semanticQuerying.Channel);
            }
        }
        else
        {
            Debug.Log("No plane detected at center of screen.");
        }
    }
}