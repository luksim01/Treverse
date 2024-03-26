using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveConnector : MonoBehaviour, IInteractive
{
    Rigidbody gameObjectsRigidbody;
    [SerializeField] private float upwardForceMultiplier = 1f;
    [SerializeField] private float rotationalForceMultiplier = 1f;
    [SerializeField] private float upwardOffset = 1f;
    GameObject kayak;

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
    }

    public void Interact()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObjectsRigidbody = gameObject.GetComponent<Rigidbody>();
        }
        else
        {
            gameObjectsRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        gameObjectsRigidbody.isKinematic = false;

        Vector3 direction = kayak.transform.position - gameObject.transform.position;
        gameObjectsRigidbody.AddForce(new Vector3(direction.x, direction.y + upwardOffset, direction.z) * upwardForceMultiplier, ForceMode.Impulse);
        gameObjectsRigidbody.AddRelativeTorque(new Vector3(0.25f, 0.5f, 1f) * rotationalForceMultiplier);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == kayak)
        {
            Destroy(gameObject);
        }
    }
}
