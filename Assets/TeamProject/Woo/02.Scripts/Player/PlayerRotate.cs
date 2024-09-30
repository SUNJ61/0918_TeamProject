using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200f;

    float mx = 0; // �¿� ȸ��
    float my = 0; // ���Ʒ� ȸ��
    public float maxYAngle = 80f; // ���Ʒ� ȸ�� ����

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 �Է°��� �޾Ƽ�
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // �¿� ȸ��
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // ���Ʒ� ȸ��
        my -= mouse_Y * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -maxYAngle, maxYAngle); // ȸ�� ���� ����

        // ȸ�� ����
        transform.eulerAngles = new Vector3(my, mx, 0);
    }
}
