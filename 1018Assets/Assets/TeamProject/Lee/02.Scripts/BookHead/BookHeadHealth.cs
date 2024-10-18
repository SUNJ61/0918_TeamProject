using System.Collections;
using UnityEngine;

public class BookHeadHealth : CretureUpdate
{
    private Animator BookHead_Animator;
    private BookHeadAI BookHead_State;

    private readonly string RespawnObj = "Respawn";
    private readonly string DieTrigger = "Die";
    private readonly string ResetBool = "Reset";
    void Awake()
    {
        BookHead_Animator = GetComponent<Animator>();
        BookHead_State = GetComponent<BookHeadAI>();
    }
    protected override void OnEnable()
    {
        base.OnEnable(); // 살아날 때마다 bool변수 초기화, hp 초기화
        BookHead_Animator.SetBool(ResetBool, false);
        BookHead_State.state = BookHeadAI.State.IDLE; //살아날때 초기화
        BookHead_State.BookHead_isDie = false; //살아날때 초기화, AI에 die 업데이트
        dead = false; //살아날때 초기화, creture에 die 업데이트
    }

    public override void OnDamage(object[] param)
    {
        //맞는 소리 클립 재생 구현 위치
        base.OnDamage(param);
    }
    public override void Die()
    {
        base.Die();
        InGameSoundManager.instance.EditSoundBox("BookHeadIdle", false); //죽을 때마다 할당된 사운드 박스제거
        InGameSoundManager.instance.Data.Remove("BookHeadIdle"); //죽으면 사운드 박스 할당할 때 사용한 key값 제거.
        
        BookHead_Animator.SetTrigger(DieTrigger);
        //총을 맞은 소리 1번만 출력하도록 넣기

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        BookHead_State.state = BookHeadAI.State.RESPAWN;
        yield return new WaitForSeconds(3.5f); //3.5초 뒤에 시체 삭제
        SpawnManager.instance.BookHeadRespawn(gameObject);
        BookHead_Animator.SetBool(ResetBool, true);
        gameObject.SetActive(false);
    }

    //private void ShowEffect(Vector3 hitPos, Vector3 hitNormal) //총을 맞은 위치 맞은 방향을 레이캐스트로 전달할 예정, 이를 통해 맞고 이펙트 효과 생성
    //{

    //}
}
