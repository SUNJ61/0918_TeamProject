using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerrMove : MonoBehaviour
{
    public float moveSpeed = 3.5f;


    CharacterController cc;
    //중력 값
    float gravity = -20f;
    // 수직 속력 변수
    float yVelocity = 0;
    //점프력
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
            dir.y = 0; // y축 이동 제거

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                // 달리기 속도 적용
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

            // CharacterController를 사용한 이동 처리
            cc.Move(dir * Time.deltaTime);
        } 
        
    }

}