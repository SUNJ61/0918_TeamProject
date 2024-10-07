using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class MissonBox : MonoBehaviour
{
    readonly string PlayerTag = "Player";

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(PlayerTag))
        {
            Pulling_Manger.instance.SetActiveTrueCandel();
            Destroy(gameObject);
        }
       
    }
}
