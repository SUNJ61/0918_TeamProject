using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampDestoryBox : MonoBehaviour
{
    [SerializeField] Transform Lamp;

    private void Start()
    {
        Lamp = GameObject.Find("Lamp").GetComponent<Transform>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(Lamp.gameObject);
            Destroy(gameObject);
        }
    }
    
}
