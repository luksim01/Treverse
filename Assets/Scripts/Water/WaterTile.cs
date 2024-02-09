using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    private MeshFilter meshFilter;
    WaterManager waterManager;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        waterManager = GameObject.Find("WaterManager").GetComponent<WaterManager>();
    }

    void FixedUpdate()
    {
        meshFilter.mesh.vertices = waterManager.vertices;
    }
}
