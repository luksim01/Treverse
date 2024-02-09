using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private float power = 3;
    [SerializeField] private float scale = 1;
    [SerializeField] private float timeScale = 1;

    private float offsetX;
    private float offsetZ;

    public Vector3[] vertices;
    private Mesh mesh;

    public GameObject waterPlane;

    void Start()
    {
        mesh = waterPlane.GetComponent<MeshFilter>().sharedMesh;
        MakeNoise();
    }

    private void FixedUpdate()
    {
        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetZ += Time.deltaTime * timeScale;
    }

    void MakeNoise()
    {
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }
    }

    float CalculateHeight(float x, float z)
    {
        float cordX = x * scale * offsetX;
        float cordZ = z * scale * offsetZ;

        return Mathf.PerlinNoise(cordX, cordZ);
    }
}
