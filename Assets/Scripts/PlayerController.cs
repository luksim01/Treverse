using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _playerModel;

    // Activating or Deactivating Player Movement During a Cutscene

    private void Awake()
    {
        instance = this;
    }


    public void Activate()
    {
        _playerMovement.enabled = true;
        _playerModel.SetActive(true);
    }

    public void Deactivate()
    {
        _playerMovement.enabled = false;
        _playerModel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
 