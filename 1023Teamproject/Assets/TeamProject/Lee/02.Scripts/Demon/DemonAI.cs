using System.Collections;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public enum State
    { //�⺻ ����, ����, �߰�, ����, �������޾� ��ٸ�, ���� 
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

            if (GameManager.G_instance.AllStop) // 1���� ��ü �Ͻ�����.
                state = State.WAIT; //���� ������Ʈ ��ü�� 4�� ����. idle ���¿��� �Ͻ�������.

            else if (Timer >= 30.0f) // 2���� ����
            {
                state = State.DIE;
                yield break; //30�� ���Ŀ� ����
            }

            else if (Demon_isDamage) // 3���� ����
                state = State.DAMAGE;

            else if (Dist < Attackside) //4���� ����
                state = State.ATTACK;

            else if (Dist < Trace_Dist) //5���� �߰�
                state = State.PLAYER_TRACE;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Action()
    {
        if(state == State.SPAWN)
        {
            demonMove.TrueDemon();
            yield return new WaitForSeconds(0.8f); //�ִϸ��̼� �ϴ� �ð� -0.2�� ��ŭ ��ٸ���.
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

                case State.DAMAGE: //�÷����� 3���̻� �¾��� ���
                    demonMove.DemonStop();
                    break;

                case State.WAIT: //�к� 6���� ���� ���
                    demonMove.IsIdle = true;
                    StopCoroutine(AddTimer()); //Ÿ�̸� ������ ��� ����.
                    yield return new WaitForSeconds(4.0f);
                    StartCoroutine(AddTimer()); //Ÿ�̸� ���� �ٽ� ����.
                    break;

                case State.DIE: //��ȯ ���ӽð��� �ٳ�����
                    demonMove.FalseDemon();
                    demon_isDie = true;
                    yield return new WaitForSeconds(4.0f);
                    break;
            }
        }
        if (!Demon_isKill) //�÷��̾ ���� ������ �ƴҰ��.
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
