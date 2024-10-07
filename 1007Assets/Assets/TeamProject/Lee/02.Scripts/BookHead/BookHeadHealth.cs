using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHeadHealth : CretureUpdate
{
    [SerializeField] private List<Transform> SpawnPoint;
    private Transform BookHead_Transform;
    private Animator BookHead_Animator;
    private BookHeadAI BookHead_State;

    private Vector3 BookHead_DiePos;

    private float Respawn_Dist;

    private readonly string RespawnObj = "Respawn";
    private readonly string DieTrigger = "Die";
    private readonly string ResetBool = "Reset";
    void Awake()
    {
        BookHead_Transform = transform;
        BookHead_Animator = GetComponent<Animator>();
        BookHead_State = GetComponent<BookHeadAI>();

        var spawn = GameObject.Find(RespawnObj).transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in spawn)
            SpawnPoint.Add(t);
        SpawnPoint.RemoveAt(0);
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
        BookHead_Animator.SetTrigger(DieTrigger);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() //스타트 코루틴은 오브젝트가 꺼지면 종료, 즉 아래 리스폰은 다른곳에다 구현해야함.
    {
        BookHead_State.state = BookHeadAI.State.RESPAWN;
        yield return new WaitForSeconds(3.5f); //3.5초 뒤에 시체 삭제
        SpawnManager.S_instance.BookHeadRespawn(SpawnPoint, gameObject);
        BookHead_Animator.SetBool(ResetBool, true);
        gameObject.SetActive(false);
    }

    private void ShowEffect(Vector3 hitPos, Vector3 hitNormal) //총을 맞은 위치 맞은 방향을 레이캐스트로 전달할 예정, 이를 통해 맞고 이펙트 효과 생성
    {

    }
}
