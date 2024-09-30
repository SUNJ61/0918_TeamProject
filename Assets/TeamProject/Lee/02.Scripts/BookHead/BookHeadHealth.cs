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
    void Awake()
    {
        BookHead_Transform = transform;
        BookHead_Animator = GetComponent<Animator>();
        BookHead_State = GetComponent<BookHeadAI>();

        var spawn = GameObject.Find("Respawn").transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in spawn)
            SpawnPoint.Add(t);
        SpawnPoint.RemoveAt(0);
    }
    protected override void OnEnable()
    {
        base.OnEnable(); // 살아날 때마다 bool변수 초기화, hp 초기화
    }

    public override void OnDamage(object[] param)
    {
        //맞는 소리 클립 재생 구현 위치
        base.OnDamage(param);
    }
    public override void Die()
    {
        base.Die();
        BookHead_Animator.SetTrigger("Die");
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3.5f); //3.5초 뒤에 시체 삭제
        gameObject.SetActive(false);
        RespawnSetup();
        yield return new WaitForSeconds(10.0f); //10초 뒤에 리스폰
        BookHead_State.state = BookHeadAI.State.IDLE;
        BookHead_State.BookHead_isDie = false; //AI에 die 업데이트
        dead = false; //creture에 die 업데이트
        gameObject.SetActive(true);
    }

    private void RespawnSetup()
    {
        Respawn_Dist = (SpawnPoint[0].position - transform.position).magnitude;
        BookHead_DiePos = transform.position;
        foreach (Transform point in SpawnPoint)
        {
            float Dist = (point.position - transform.position).magnitude;
            if (Dist > Respawn_Dist)
            {
                BookHead_Transform.position = point.position;
            }
        }
    }

    private void ShowEffect(Vector3 hitPos, Vector3 hitNormal) //총을 맞은 위치 맞은 방향을 레이캐스트로 전달할 예정, 이를 통해 맞고 이펙트 효과 생성
    {

    }
}
