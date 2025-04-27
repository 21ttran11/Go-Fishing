using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAnalyser : MonoBehaviour
{
    [SerializeField]
    private float groundThreshold;

    [SerializeField]
    private float avgNormal;

    [SerializeField]
    private float minVerts;

    [SerializeField]
    private bool isGround;

    public bool IsGround
    {
        get => isGround;
    }

    public float AvgNorm
    {
        get => avgNormal;
    }

    public event Action analysisDone;

    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if(meshFilter != null)
        {
            StartCoroutine(CheckForGround());
        }
    }

    IEnumerator CheckForGround()
    {
        yield return new WaitUntil(() =>
        {
            return meshFilter.sharedMesh.vertices.Length > minVerts;
        });

        isGround = AnalyseForGround(meshFilter.sharedMesh);
        analysisDone?.Invoke();
    }

    bool AnalyseForGround(Mesh mesh)
    {
        float averageVert = 0;

        foreach (var normal in mesh.normals)
        {
            averageVert += normal.normalized.y;
        }

        averageVert /= mesh.vertices.Length;
        avgNormal = averageVert;

        if (averageVert >= groundThreshold)
        {
            return true;
        }

        return false;
    }
}
