using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController Player_Controller;
    private Transform Camera_Pivot; //ī�޶� x�� ȸ��
    private Transform Player_Transform;

    private Vector3 Velocity_y = Vector3.zero; //�߷�

    private float moveSpeed;
    private float currentXRot = 0f;

    private readonly float rotSpeed = 10.0f;
    private readonly float Gravity = -25f;
    private readonly float JumpHeight = 0.4f;

    private Vector3 playerDir = Vector3.zero; //�̵� ����
    public Vector3 PlayerDir
    { 
        get { return playerDir; } 
        set { playerDir = value; } 
    }
    private Vector2 playerRot = Vector2.zero; //ȸ�� ����
    public Vector2 PlayerRot
    {
        get { return playerRot; }
        set { playerRot = value; }
    }
    private bool player_isRun; //�޸��� ���� üũ
    public bool Player_isRun
    {
        get { return player_isRun; }
        set { player_isRun = value; }
    }
    private bool Player_isJump;
    public bool Player_IsJump
    {
        get { return Player_isJump; }
        set
        {
            Player_isJump = value;
        }
    }
    private bool isGround;
    public bool IsGround
    {
        get { return isGround; }
        set { isGround = value; }
    }

    void Awake()
    {
        Player_Transform = transform;
        Player_Controller = GetComponent<CharacterController>();
        Camera_Pivot = transform.GetChild(0).GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        CheckJumpState();
        Player_Moving();
        Camera_Moving();
    }

    private void Player_Moving()
    {
        if (!Player_isRun)
            moveSpeed = 3.0f;
        else if (Player_isRun && PlayerDir.z >= 1.0f) //wŰ�� ������ �� �޸���
            moveSpeed = 6.0f; 
        else if (Player_isRun && PlayerDir.z >= 0.7f) //w�� ad Ű�� ������ �� �޸���
            moveSpeed = 5.0f;

        Vector3 move = transform.TransformDirection(PlayerDir);
        Player_Controller.Move(move * moveSpeed * Time.deltaTime);
    }
    private void CheckJumpState()
    {
        if (IsGround)
        {
            Velocity_y.y = 0;
            if (Player_IsJump)
            {
                Velocity_y.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        else
        {
            Velocity_y.y += Gravity * Time.deltaTime;
            Player_IsJump = false;
        }
        Player_Controller.Move(Velocity_y * Time.deltaTime);
    }
    private void Camera_Moving()
    {
        Player_Transform.Rotate(Vector3.up * PlayerRot.x * rotSpeed * Time.deltaTime);

        currentXRot -= playerRot.y * rotSpeed * Time.deltaTime;
        currentXRot = Mathf.Clamp(currentXRot, -45f, 45f);
        Camera_Pivot.localRotation = Quaternion.Euler(currentXRot, 0f, 0f);
    }
}
