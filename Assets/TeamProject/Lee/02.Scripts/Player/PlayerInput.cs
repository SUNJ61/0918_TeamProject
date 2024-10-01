using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMove playerMove;
    private UseItem playerItem;
    private CamerRay playerAction;

    [SerializeField]private Vector3 Player_Dir;
    [SerializeField]private Vector2 Player_Rot;

    [SerializeField]private float Player_RunState;
    [SerializeField]private float Player_JumpState;
    [SerializeField]private float Player_FireState;
    [SerializeField]private float Player_CatchState;
    [SerializeField]private float Player_ActionState;

    [SerializeField]private bool Player_isRun;
    [SerializeField]private bool Player_isJump;
    [SerializeField]private bool Player_isFire;
    [SerializeField]private bool Player_isCatch;
    [SerializeField]private bool Player_isAction;
    
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerItem = GetComponent<UseItem>();
        playerAction = transform.GetChild(0).GetComponent<CamerRay>();
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        CheckState();
        UpdateState();
    }

    private void UpdateState()
    {
        //이동 관련 업데이트
        playerMove.PlayerDir = Player_Dir;
        playerMove.PlayerRot = Player_Rot;
        playerMove.Player_isRun = Player_isRun;
        playerMove.Player_IsJump = Player_isJump;

        //아이템 관련 업데이트
        playerItem.IsUse = Player_isFire;

        //상호작용 관련 업데이트
        playerAction.IsCatch = Player_isCatch;
        playerAction.IsAction = Player_isAction;
    }

    private void CheckState()
    {
        if (Player_RunState > 0.1f && Player_Dir.z >= 0.7f) //달리기 입력감지
            Player_isRun = true;
        else
            Player_isRun = false;

        if (Player_FireState != 0) //총쏘기 입력 감지
            Player_isFire = true;
        else
            Player_isFire = false;

        if (Player_JumpState != 0) //점프 입력 감지
            Player_isJump = true;
        else
            Player_isJump = false;

        if(Player_CatchState != 0) //줍기 입력 감지
            Player_isCatch = true;
        else
            Player_isCatch= false;

        if(Player_ActionState != 0)
            Player_isAction = true;
        else
            Player_isAction= false;
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

    private void OnCatch(InputValue value)
    {
        Player_CatchState = value.Get<float>();
    }

    private void OnAction(InputValue value)
    {
        Player_ActionState = value.Get<float>();
    }
}
