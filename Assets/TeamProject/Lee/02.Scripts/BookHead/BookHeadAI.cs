using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHeadAI : MonoBehaviour
{
    private Transform Player_Transform;
    private Transform BookHead_Transform;
    private CapsuleCollider BookHead_CapCol;
    private Rigidbody BookHead_RBody;
    private Animator BookHead_animator;

    private BookHeadMovement BookHead_Movement;
    private BookHeadAttack BookHead_Attack;

    private Vector3 TracePoint;

    private float dist;
    private float TraceVect_dist;
    private float Attack_dist = 2.0f;
    private float OnTrace = 10.0f;

    private readonly string playerTag = "Player";

    private bool bookHead_isDie;
    public bool BookHead_isDie
    {
        get { return bookHead_isDie; }
        set { bookHead_isDie = value; }
    }

    public enum State
    {
        IDLE = 0, POINT_TRACE, PLAYER_TRACE, ATTACK, RESPAWN
    }
    public State state = State.IDLE;
    void Awake()
    {
        Player_Transform = GameObject.FindWithTag(playerTag).GetComponent<Transform>();
        BookHead_Transform = GetComponent<Transform>();
        BookHead_CapCol = GetComponent<CapsuleCollider>();
        BookHead_RBody = GetComponent<Rigidbody>();
        BookHead_animator = GetComponent<Animator>();
        BookHead_Movement = GetComponent<BookHeadMovement>();
        BookHead_Attack = GetComponent<BookHeadAttack>();
    }
    private void OnEnable()
    {
        TracePoint = Player_Transform.position;

        StartCoroutine(CheckState()); //������ ���� ������Ʈ
        StartCoroutine(UpdatePlayerTransform()); //�÷��̾� ��ġ�� 10�ʸ��� ������Ʈ
        StartCoroutine(Action()); //state�� ���� ���� ������Ʈ
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f); // �ʱ� ������ٸ���
        while(!BookHead_isDie) //bookhead�� ���� �� ���� �ݺ�
        {
            if(state == State.RESPAWN) yield break;
            dist = (Player_Transform.position - BookHead_Transform.position).magnitude;
            TraceVect_dist = (BookHead_Transform.position - TracePoint).magnitude;

            if (dist < Attack_dist) //������ �ֿ켱
            {
                state = State.ATTACK;
            }

            else if (dist < OnTrace) //�÷��̾� �ѱⰡ 2����
            {
                state = State.PLAYER_TRACE;
            }

            else if (TraceVect_dist < 1.5f) //�÷��̾� �߰��� ���� ���� ������Ʈ�� 3����
            {
                state = State.IDLE;
            }

            else if (dist > OnTrace) //�÷��̾� ���� ��ġ �߰��� 4����
            {
                state = State.POINT_TRACE;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.5f); // �ʱ� ������ٸ���

        while (!BookHead_isDie) //bookhead�� ���� �� ���� �ݺ�
        {
            yield return new WaitForSeconds(0.2f);

            switch(state)
            {
                case State.IDLE:
                    BookHead_Movement.IsIdle = true;
                    yield return new WaitForSeconds(2.0f);
                    TracePoint = Player_Transform.position;
                    break;

                case State.POINT_TRACE:
                    BookHead_Movement.Point_Trace = TracePoint;
                    break;

                case State.PLAYER_TRACE:
                    BookHead_Movement.Player_Trace = Player_Transform.position;
                    break;

                case State.ATTACK:
                    BookHead_Attack.isAttack = true;
                    yield return new WaitForSeconds(1.8f);
                    break;

                case State.RESPAWN:
                    BookHead_isDie = true;
                    break;
            }
        }
    }

    IEnumerator UpdatePlayerTransform()
    {
        yield return new WaitForSeconds(0.5f); // �ʱ� ������ٸ���

        while (!GameManager.G_instance.isGameover)
        {
            TracePoint = Player_Transform.position;
            yield return new WaitForSeconds(20.0f);
        }
    }
}
