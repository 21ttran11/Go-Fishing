using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Niantic.Lightship.AR.Semantics; 
public class SpawnOnARMesh : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private float minVertsForSpawn;
    [SerializeField] private float scaler;

    [Range(0, 100)]
    [SerializeField] private int spawnLikelyHood = 33;
    private GameObject spawnedObject;

    private MeshAnalyser meshAnalyser;
    private Mesh arMesh;
    private SemanticQuerying _semanticQuerying;  

    void Start()
    {
        if (spawnLikelyHood == 0) return;

        meshAnalyser = GetComponent<MeshAnalyser>();
        _semanticQuerying = FindObjectOfType<SemanticQuerying>(); 

        meshAnalyser.analysisDone += StartSpawning;
    }

    private void OnDestroy()
    {
        meshAnalyser.analysisDone -= StartSpawning;
    }

    void StartSpawning()
    {
        arMesh = GetComponent<MeshFilter>().sharedMesh;

        int spawnLikely = Random.Range(0, 100 / spawnLikelyHood);
        Debug.Log("Spawnlikely => " + spawnLikely);

        if (spawnLikely != 0)
        {
            return;
        }

        if (arMesh.vertexCount > minVertsForSpawn &&
            meshAnalyser.IsGround)
        {
            if (_semanticQuerying != null && _semanticQuerying.Channel == "ground")
            {
                InstantiateObject(GetRandomObject());
            }
        }
    }

    GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }

    void InstantiateObject(GameObject obj)
    {
        Vector3 spawnPosition = GetRandomVector();
        if (spawnPosition != Vector3.zero)
        {
            spawnedObject = Instantiate(obj, spawnPosition, Quaternion.identity);
            spawnedObject.transform.localScale *= scaler;
        }
    }

    Vector3 GetRandomVector()
    {
        Vector3 highestVert = Vector3.zero;
        float highestY = Mathf.NegativeInfinity;

        foreach (var vert in arMesh.vertices)
        {
            if (vert.y > highestY)
            {
                highestY = vert.y;
                highestVert = transform.TransformPoint(vert);
            }
        }

        return highestVert;
    }
}