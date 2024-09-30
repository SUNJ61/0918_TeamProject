using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove playerMove;
    private UseItem playerItem;

    [SerializeField]private Vector3 Player_Dir;
    [SerializeField]private Vector2 Player_Rot;

    [SerializeField]private float Player_RunState;
    [SerializeField]private float Player_JumpState;
    [SerializeField]private float Player_FireState;

    [SerializeField]private bool Player_isRun;
    [SerializeField] private bool Player_isJump;
    [SerializeField]private bool Player_isFire;
    
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerItem = GetComponent<UseItem>();
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        CheckState();

        playerMove.PlayerDir = Player_Dir;
        playerMove.PlayerRot = Player_Rot;
        playerMove.Player_isRun = Player_isRun;
        playerMove.Player_IsJump = Player_isJump;
        playerItem.IsFire = Player_isFire;
    }

    private void CheckState()
    {
        if (Player_RunState > 0.1f && Player_Dir.z >= 0.7f)
            Player_isRun = true;
        else
            Player_isRun = false;

        if (Player_FireState != 0)
            Player_isFire = true;
        else
            Player_isFire = false;

        if (Player_JumpState != 0)
            Player_isJump = true;
        else
            Player_isJump = false;
    }

    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        Player_Dir = new Vector3(dir.x, 0f, dir.y).normalized;
    }

    private void OnLook(InputValue value)
    {
        Player_Rot = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        Player_RunState = value.Get<float>();
    }
    private void OnJump(InputValue value)
    {
        Player_JumpState = value.Get<float>();
    }
    private void OnFire(InputValue value)
    {
        Player_FireState = value.Get<float>();
    }
}
