using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private GameObject kayak;
    public GameObject waterTile;
    public bool isPlaneSpawned = false;

    [SerializeField] private float power = 0.6f;
    [SerializeField] private float scale = 0.2f;
    [SerializeField] private float timeScale = 1f;

    private float offsetX;
    private float offsetZ;

    public Vector3[] vertices;
    private Mesh mesh;

    void Start()
    {
        kayak = GameObject.Find("Kayak");
        mesh = waterTile.GetComponent<MeshFilter>().sharedMesh;
        MakeNoise();
    }

    void Update()
    {
        if (kayak.transform.position.z > 5 && !isPlaneSpawned)
        {
            isPlaneSpawned = true;
            Instantiate(waterTile, new Vector3(0, 0, 10), waterTile.transform.rotation);
        }
    }

    void FixedUpdate()
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
