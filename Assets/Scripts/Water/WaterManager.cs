using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private GameObject kayak;
    public GameObject waterTile;
    public bool isPlaneSpawned = false;

    // wave parameters
    [SerializeField] private float power = 2f;
    [SerializeField] private float scale = 0.5f;
    [SerializeField] private float timeScale = 0.5f;

    // wave generation
    private float offsetX;
    private float offsetZ;
    private Vector3[] vertices;
    private Mesh waterTileMesh;

    // water tile coordination
    public class TilePosition
    {
        public Vector3 currentPosition;
        public Vector3 previousPosition;

        public TilePosition(Vector3 position)
        {
            currentPosition = position;
        }

        public void UpdatePosition(Vector3 position)
        {
            previousPosition = currentPosition;
            currentPosition = position;
        }

        public bool HasChangedPosition()
        {
            if (currentPosition != previousPosition)
            {
                return true;
            }
            return false;
        }

        public Vector3 GetPosition()
        {
            return currentPosition;
        }
    }
    public TilePosition centerWaterTile = new TilePosition(new Vector3(0, 0, 0));
    [SerializeField] private int waterTileGridSize = 5;
    [SerializeField] private int waterTileLengthX;
    [SerializeField] private int waterTileLengthZ;
    private List<GameObject> waterTilePool;
    private int amountToPool;
    private bool[,] waterTileEmptyStatus;
    private Vector3[,] waterTileOffset;
    private Vector3[,] waterTilePosition;

    // water colour
    private Renderer waterTileRenderer;
    [SerializeField] private Color waterColourCurrent = new Color();

    void Start()
    {
        kayak = GameObject.Find("Kayak");

        GameObject.Find("WaterLevelIndicator").SetActive(false);

        waterTileRenderer = waterTile.GetComponent<Renderer>();
        waterTileMesh = waterTile.GetComponent<MeshFilter>().sharedMesh;
        waterTileLengthX = (int)GetTileScale().x * (int)waterTileMesh.bounds.size.x;
        waterTileLengthZ = (int)GetTileScale().z * (int)waterTileMesh.bounds.size.z;

        // initialising grid of water tile empty statuses
        waterTileEmptyStatus = new bool[waterTileGridSize, waterTileGridSize];
        for (int x = 0; x < waterTileEmptyStatus.GetLength(1); x++)
        {
            for (int z = 0; z < waterTileEmptyStatus.GetLength(0); z++)
            {
                waterTileEmptyStatus[x, z] = true;
            }
        }

        // initiliasing grid of water tile offsets
        int gridOffsetX, gridOffsetZ, gridOffset = Mathf.FloorToInt(waterTileGridSize / 2);
        waterTileOffset = new Vector3[waterTileGridSize, waterTileGridSize];
        for (int x = 0; x < waterTileOffset.GetLength(1); x++)
        {
            gridOffsetX = x - gridOffset;
            for (int z = 0; z < waterTileOffset.GetLength(0); z++)
            {
                gridOffsetZ = z - gridOffset;
                // water tile offsets calculated using water tile size
                waterTileOffset[x, z] = new Vector3(waterTileLengthX * gridOffsetX, 0, waterTileLengthZ * gridOffsetZ);
            }
        }

        // initialise grid of water tile global position coordinates
        waterTilePosition = new Vector3[waterTileGridSize, waterTileGridSize];

        // creating a pool of water tiles to coordinate
        waterTilePool = new List<GameObject>();
        GameObject waterTileToPool;
        amountToPool = waterTileGridSize * waterTileGridSize;
        for (int i = 0; i < amountToPool; i++)
        {
            // instantiate water tiles as children of the manager
            waterTileToPool = Instantiate(waterTile, this.transform);
            waterTileToPool.SetActive(false);
            waterTilePool.Add(waterTileToPool);
        }
        // coordinate water tiles around the player from the pool
        PopulateWaterTilesFromPool(amountToPool, new Vector3(0, 0, 0));

        // water colour
        waterColourCurrent = waterTileRenderer.sharedMaterial.GetColor("_EmissionColor");

        // create waves
        MakeNoise();
    }

    void Update()
    {
        // update global coodinates of the center of the water tile that the player is located on
        centerWaterTile.UpdatePosition(GetCenterWaterTileCoords());

        // when the location of the water tile that the player is on updates
        if(centerWaterTile.HasChangedPosition())
        {
            // return the farthest water tiles behind the player movement direction back into the water tile pool
            int amountInPool = ReturnWaterTilesToPool(centerWaterTile.GetPosition());

            // then reissue the water tiles from the pool ahead of player movement direction past the furthest water tiles
            PopulateWaterTilesFromPool(amountInPool, centerWaterTile.GetPosition());
        }
    }

    void FixedUpdate()
    {
        MakeNoise();
        offsetX += Time.deltaTime * timeScale;
        offsetZ += Time.deltaTime * timeScale;
    }

    private Vector3 GetCenterWaterTileCoords()
    {
        Vector3 centerWaterTilePosition = new();

        if (kayak.transform.position.x >= 0)
        {
            centerWaterTilePosition.x = Mathf.FloorToInt(kayak.transform.position.x / waterTileLengthX) * waterTileLengthX;
        }
        else if (kayak.transform.position.x < 0)
        {
            centerWaterTilePosition.x = Mathf.CeilToInt(kayak.transform.position.x / waterTileLengthX) * waterTileLengthX;
        }
        if (kayak.transform.position.z > 0)
        {
            centerWaterTilePosition.z = Mathf.FloorToInt(kayak.transform.position.z / waterTileLengthZ) * waterTileLengthZ;
        }
        else if (kayak.transform.position.z < 0)
        {
            centerWaterTilePosition.z = Mathf.CeilToInt(kayak.transform.position.z / waterTileLengthZ) * waterTileLengthZ;
        }

        return centerWaterTilePosition;
    }

    private int ReturnWaterTilesToPool(Vector3 centerWaterTilePosition)
    {
        int inverseX, inverseZ, numTilesBeforeClear = Mathf.CeilToInt((float)waterTileGridSize / 2), amountInPool = 0;
        for (int i = 0; i < amountToPool; i++)
        {
            // farthest water tiles from new center water tile identified
            if (Mathf.Abs(centerWaterTilePosition.x - waterTilePool[i].transform.position.x) >= (numTilesBeforeClear * waterTileLengthX) ||
                Mathf.Abs(centerWaterTilePosition.z - waterTilePool[i].transform.position.z) >= (numTilesBeforeClear * waterTileLengthZ))
            {
                for (int x = 0; x < waterTileOffset.GetLength(1); x++)
                {
                    for (int z = 0; z < waterTileOffset.GetLength(0); z++)
                    {
                        // flag positions ahed of player movement direction past the furthest water tiles as empty
                        if (waterTilePool[i].transform.position == waterTilePosition[x, z])
                        {
                            inverseX = waterTileOffset.GetUpperBound(1) - x;
                            inverseZ = waterTileOffset.GetUpperBound(0) - z;
                            waterTileEmptyStatus[inverseX, inverseZ] = true;
                        }
                    }
                }
                // farthest water tiles from new center water tile returned to water tile pool
                waterTilePool[i].SetActive(false);
                amountInPool++;
            }
        }
        return amountInPool;
    }

    private void PopulateWaterTilesFromPool(int amountInPool, Vector3 centerWaterTilePosition)
    {
        for (int i = 0; i < amountInPool; i++)
        {
            for (int x = 0; x < waterTileOffset.GetLength(1); x++)
            {
                for (int z = 0; z < waterTileOffset.GetLength(0); z++)
                {
                    // new position to populate with a water tile from the water tile pool 
                    waterTilePosition[x, z] = centerWaterTilePosition + waterTileOffset[x, z];
                    GameObject pooledWaterTile = GetPooledWaterTile();
                    if (pooledWaterTile != null && waterTileEmptyStatus[x, z])
                    {
                            waterTileEmptyStatus[x, z] = false;
                            pooledWaterTile.transform.SetPositionAndRotation(waterTilePosition[x, z], waterTile.transform.rotation);
                            pooledWaterTile.SetActive(true);
                    }
                }
            }
        }
    }

    private GameObject GetPooledWaterTile()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!waterTilePool[i].activeInHierarchy)
            {
                return waterTilePool[i];
            }
        }
        return null;
    }

    private void MakeNoise()
    {
        vertices = waterTileMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }
    }

    private float CalculateHeight(float x, float z)
    {
        float cordX = x * scale * offsetX;
        float cordZ = z * scale * offsetZ;

        return Mathf.PerlinNoise(cordX, cordZ);
    }

    public Vector3[] GetVertices()
    {
        return vertices;
    }

    public Vector3 GetTileScale()
    {
        return waterTile.transform.localScale;
    }

    public Vector3 GetTileLength()
    {
        return new Vector3(waterTileLengthX, 0 , waterTileLengthZ);
    }

    public void UpdateWaterTileColour(Color waterColour){
        for(int i = 0; i < amountToPool; i++){
            waterTilePool[i].GetComponent<Renderer>().material.SetColor("_ShallowColor", waterColour);
            Color darkColour = convertToDeepColour(waterColour);
            waterTilePool[i].GetComponent<Renderer>().material.SetColor("_DeepColor", darkColour);
            //Color foamColour = convertToFoamColour(waterColour);
            //waterTilePool[i].GetComponent<Renderer>().material.SetColor("_FoamColor", foamColour);
        }
    }

    public bool InitiateWaterTileColourChangeTo(Color waterColourTarget, float changeSpeed)
    {
        if (waterColourCurrent != waterColourTarget)
        {
            float vectorLength = Mathf.Sqrt((waterColourTarget[0] * waterColourCurrent[0]) +
                                            (waterColourTarget[1] * waterColourCurrent[1]) +
                                            (waterColourTarget[2] * waterColourCurrent[2]) +
                                            (waterColourTarget[3] * waterColourCurrent[3]));

            float rNormalized = (waterColourTarget[0] - waterColourCurrent[0]) / vectorLength;
            float bNormalized = (waterColourTarget[1] - waterColourCurrent[1]) / vectorLength;
            float gNormalized = (waterColourTarget[2] - waterColourCurrent[2]) / vectorLength;
            float aNormalized = (waterColourTarget[3] - waterColourCurrent[3]) / vectorLength;

            Color waterColourDelta = new Color(rNormalized, bNormalized, gNormalized, aNormalized) * changeSpeed * Time.deltaTime;
            waterColourCurrent += waterColourDelta;
            UpdateWaterTileColour(waterColourCurrent);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Color convertToFoamColour(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return Color.HSVToRGB(H, 0.1f * S, V);
    }
    private Color convertToDeepColour(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return Color.HSVToRGB(H, S, V * 0.5f);
    }
}
