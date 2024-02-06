using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSomeNoise : MonoBehaviour
{
    [SerializeField] private float power = 3;
    [SerializeField] private float scale = 1;
    [SerializeField] private float timeScale = 1;

    private float offsetX;
    private float offsetZ;
    private MeshFilter meshFilter;

    public Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        MakeNoise();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetZ += Time.deltaTime * timeScale;
    }

    void MakeNoise()
    {
        vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }

        meshFilter.mesh.vertices = vertices;
    }

    float CalculateHeight(float x, float z)
    {
        float cordX = x * scale * offsetX;
        float cordZ = z * scale * offsetZ;

        return Mathf.PerlinNoise(cordX, cordZ);
    }
}
