using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamerRotate : MonoBehaviour
{
    private float PlayerSpeed = 500f;
    // Start is called before the first frame update
    float mx = 0f;
    float my = 0f;
    Vector3 CamTr;
    private Transform TagetTr;
    private float heightOffset = 0.5f;
    void Start()
    {
        CamTr = Vector3.up * 3f;
        TagetTr = GameObject.Find("Player").transform;
        
    }

    void Update()
    {
        CameraRotant(false);
    }

    public void CameraRotant(bool isdie)
    {
        if (!isdie)
        {
            Vector3 desiredPosition = TagetTr.position + Vector3.up * heightOffset;
            transform.position = desiredPosition;
            float mouse_X = Input.GetAxis("Mouse X");
            float mouse_Y = Input.GetAxis("Mouse Y");


            mx += mouse_X * PlayerSpeed * Time.deltaTime;
            my += mouse_Y * PlayerSpeed * Time.deltaTime;

            my = Mathf.Clamp(my, -60f, 60f);

            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
       
    }
}
