using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantPoint : MonoBehaviour
{
    public Rigidbody rigidBody;

    // turbulence
    KayakController kayakController;
    private float waterTileCenterToSideLengthX;
    private float waterTileCenterToSideLengthZ;
    [SerializeField] private float waterDrag;
    [SerializeField] private float waterAngularDrag;

    void Start()
    {
        kayakController = GameObject.Find("Kayak").GetComponent<KayakController>();
        waterTileCenterToSideLengthX = kayakController.waterTileCenterToSideLengthX;
        waterTileCenterToSideLengthZ = kayakController.waterTileCenterToSideLengthZ;
    }

    void FixedUpdate()
    {
        GameObject waterTile = ChooseWaterTileForTurbulenceFrom(kayakController.waterTilesInContact);
        float waterLevel = GetWaterLevelOf(waterTile);
        FindTurbulenceUsing(waterLevel);
    }

    private GameObject ChooseWaterTileForTurbulenceFrom(List<GameObject> waterTilesInContact)
    {
        GameObject waterTile = null;

        // choose a water tile that buoyant point is overlapping
        for (int i = 0; i < waterTilesInContact.Count; i++)
        {
            float waterTilePosX = waterTilesInContact[i].GetComponent<WaterTile>().GetCenterPosition().x;
            float waterTilePosZ = waterTilesInContact[i].GetComponent<WaterTile>().GetCenterPosition().z;
            if (transform.position.x > waterTilePosX - waterTileCenterToSideLengthX &&
                transform.position.x < waterTilePosX + waterTileCenterToSideLengthX &&
                transform.position.z > waterTilePosZ - waterTileCenterToSideLengthZ &&
                transform.position.z < waterTilePosZ + waterTileCenterToSideLengthZ)
            {
                waterTile = waterTilesInContact[i];
            }
        }
        return waterTile;
    }

    private void FindTurbulenceUsing(float waterLevel)
    {
        rigidBody.AddForceAtPosition(Physics.gravity / 4, transform.position, ForceMode.Acceleration);

        // buoyant point is pushed around by changes in water level
        if (transform.position.y < waterLevel)
        {
            float displacementMultiplier = waterLevel - transform.position.y;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    private float GetWaterLevelOf(GameObject waterTile)
    {
        // get the water level at the buoyant point position
        float waterLevel = 0.5f;

        if (waterTile != null)
        {
            Vector3[] waterVertices;
            waterVertices = waterTile.GetComponent<MeshFilter>().sharedMesh.vertices;
            
            for (int i = 0; i < waterVertices.Length; i++)
            {
                
                // vertices transformed from local to world coordinates
                waterVertices[i] = waterTile.transform.TransformPoint(waterVertices[i]);

                // water level within water tile vertices proximity
                if (transform.position.x < waterVertices[i].x + (waterTile.transform.localScale.x) &&
                    transform.position.x > waterVertices[i].x - (waterTile.transform.localScale.x) &&
                    transform.position.z < waterVertices[i].z + (waterTile.transform.localScale.z) &&
                    transform.position.z > waterVertices[i].z - (waterTile.transform.localScale.z))
                {
                    waterLevel = waterVertices[i].y + 0.5f;
                }
            }
        }
        return waterLevel;
    }
}
