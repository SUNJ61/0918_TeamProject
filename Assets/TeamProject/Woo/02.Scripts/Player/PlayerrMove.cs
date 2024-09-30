using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerrMove : MonoBehaviour
{
    public float moveSpeed = 3.5f;


    CharacterController cc;
    //�߷� ��
    float gravity = -20f;
    // ���� �ӷ� ����
    float yVelocity = 0;
    //������
    public float Jump = 2f;
    public bool isJumping = false;
    private void Start()
    {
        cc = GetComponent<CharacterController>();

    }
    void Update()
    {
        PlayerMoveJump(false);

    }

    private void PlayerMoveJump(bool isdie)
    {
        if (!isdie)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0; // y�� �̵� ����

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                // �޸��� �ӵ� ����
                dir *= (moveSpeed * 2f);
            }
            else
            {
                dir *= moveSpeed;
            }



            if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = Jump;
                isJumping = true;
            }
            else if (cc.isGrounded && !isJumping)
            {
                yVelocity = -1;
            }
            else
            {
                yVelocity += gravity * Time.deltaTime;
            }

            dir.y = yVelocity;

            // CharacterController�� ����� �̵� ó��
            cc.Move(dir * Time.deltaTime);
        } 
        
    }

}