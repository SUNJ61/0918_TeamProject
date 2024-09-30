using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200f;

    float mx = 0; // 좌우 회전
    float my = 0; // 위아래 회전
    public float maxYAngle = 80f; // 위아래 회전 범위

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 입력값을 받아서
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 좌우 회전
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // 위아래 회전
        my -= mouse_Y * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -maxYAngle, maxYAngle); // 회전 범위 제한

        // 회전 적용
        transform.eulerAngles = new Vector3(my, mx, 0);
    }
}
