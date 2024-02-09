using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTile : MonoBehaviour
{
    private MeshFilter meshFilter;
    WaveGenerator waveGenerator;
    public Vector3[] vertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        waveGenerator = GameObject.Find("WaveGenerator").GetComponent<WaveGenerator>();
    }

    void FixedUpdate()
    {
        meshFilter.mesh.vertices = waveGenerator.vertices;
    }
}
