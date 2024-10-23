using System.Collections;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public enum State
    { //기본 상태, 스폰, 추격, 공격, 데미지받아 기다림, 멈춤 
        IDLE = 0, SPAWN, PLAYER_TRACE, ATTACK, DAMAGE, WAIT, DIE
    }
    public State state = State.SPAWN;

    private Transform PlayerPos;
    private Transform DemonPos;

    private DemonMove demonMove;
    private DemonAttack demonAttack;

    private float Timer = 0f;
    private float Dist;

    readonly float Trace_Dist = 15f;
    readonly float Attackside = 3f;

    private bool demon_isDie;
    public bool Demon_isDie
    {
        get { return demon_isDie; }
        set {  demon_isDie = value; }
    }

    private bool demon_isKill;
    public bool Demon_isKill
    {
        get { return demon_isKill; }
        set { demon_isKill = value; }
    }

    private bool demon_isDamage;
    public bool Demon_isDamage
    {
        get { return demon_isDamage; }
        set { demon_isDamage = value; }
    }

    private void Awake()
    {
        DemonPos = transform;
        PlayerPos = GameObject.Find("Player").transform;
        demonMove = GetComponent<DemonMove>();
        demonAttack = GetComponent<DemonAttack>();
    }
    private void OnEnable()
    {
        if (GameManager.G_instance.isGameStart)
        {
            state = State.SPAWN;
            Timer = 0f;

            StartCoroutine(CheckState());
            StartCoroutine(Action());
            StartCoroutine(AddTimer());
        }
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        while(!Demon_isDie && !GameManager.G_instance.isGameover)
        {
            Dist = (PlayerPos.position - DemonPos.position).magnitude;

            if (GameManager.G_instance.AllStop) // 1순위 전체 일시정지.
                state = State.WAIT; //상태 업데이트 자체가 4초 멈춤. idle 상태에서 일시정지됨.

            else if (Timer >= 30.0f) // 2순위 디스폰
            {
                state = State.DIE;
                yield break; //30초 이후에 디스폰
            }

            else if (Demon_isDamage) // 3순위 스턴
                state = State.DAMAGE;

            else if (Dist < Attackside) //4순위 공격
                state = State.ATTACK;

            else if (Dist < Trace_Dist) //5순위 추격
                state = State.PLAYER_TRACE;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Action()
    {
        if(state == State.SPAWN)
        {
            demonMove.TrueDemon();
            yield return new WaitForSeconds(0.8f); //애니메이션 하는 시간 -0.2초 만큼 기다린다.
        }
            
        while(!Demon_isDie && !GameManager.G_instance.isGameover)
        {
            yield return new WaitForSeconds(0.2f);

            switch(state)
            {
                case State.IDLE:
                    demonMove.IsIdle = true;
                    break;

                case State.PLAYER_TRACE:
                    demonMove.IsTrace = true;
                    break;

                case State.ATTACK:
                    demonAttack.IsAttack = true;
                    yield return new WaitForSeconds(1.2f);
                    break;

                case State.DAMAGE: //플래쉬를 3초이상 맞았을 경우
                    demonMove.DemonStop();
                    break;

                case State.WAIT: //촛불 6개를 켰을 경우
                    demonMove.IsIdle = true;
                    StopCoroutine(AddTimer()); //타이머 증가를 잠깐 멈춤.
                    yield return new WaitForSeconds(4.0f);
                    StartCoroutine(AddTimer()); //타이머 증가 다시 시작.
                    break;

                case State.DIE: //소환 지속시간이 다끝나면
                    demonMove.FalseDemon();
                    demon_isDie = true;
                    yield return new WaitForSeconds(4.0f);
                    break;
            }
        }
        if (!Demon_isKill) //플레이어를 죽인 데몬이 아닐경우.
            demonMove.PlayerDie();
    }

    IEnumerator AddTimer()
    {
        while (Timer <= 30.0f)
        {
            yield return new WaitForSeconds(0.1f);
            Timer += 0.1f;
        }
    }
}
