using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackKayak : MonoBehaviour
{
    GameObject kayak;

    private void Start()
    {
        kayak = GameObject.Find("Kayak");
    }

    void FixedUpdate()
    {
        transform.SetPositionAndRotation(kayak.transform.position, Quaternion.Euler(0, kayak.transform.rotation.eulerAngles.y, 0));
    }
}
