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
        base.OnEnable(); // ��Ƴ� ������ bool���� �ʱ�ȭ, hp �ʱ�ȭ
    }

    public override void OnDamage(object[] param)
    {
        //�´� �Ҹ� Ŭ�� ��� ���� ��ġ
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
        yield return new WaitForSeconds(3.5f); //3.5�� �ڿ� ��ü ����
        gameObject.SetActive(false);
        RespawnSetup();
        yield return new WaitForSeconds(10.0f); //10�� �ڿ� ������
        BookHead_State.state = BookHeadAI.State.IDLE;
        BookHead_State.BookHead_isDie = false; //AI�� die ������Ʈ
        dead = false; //creture�� die ������Ʈ
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

    private void ShowEffect(Vector3 hitPos, Vector3 hitNormal) //���� ���� ��ġ ���� ������ ����ĳ��Ʈ�� ������ ����, �̸� ���� �°� ����Ʈ ȿ�� ����
    {

    }
}
