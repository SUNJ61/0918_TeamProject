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
        base.OnEnable(); // ��Ƴ� ������ bool���� �ʱ�ȭ, hp �ʱ�ȭ
        BookHead_Animator.SetBool(ResetBool, false);
        BookHead_State.state = BookHeadAI.State.IDLE; //��Ƴ��� �ʱ�ȭ
        BookHead_State.BookHead_isDie = false; //��Ƴ��� �ʱ�ȭ, AI�� die ������Ʈ
        dead = false; //��Ƴ��� �ʱ�ȭ, creture�� die ������Ʈ
    }

    public override void OnDamage(object[] param)
    {
        //�´� �Ҹ� Ŭ�� ��� ���� ��ġ
        base.OnDamage(param);
    }
    public override void Die()
    {
        base.Die();
        BookHead_Animator.SetTrigger(DieTrigger);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() //��ŸƮ �ڷ�ƾ�� ������Ʈ�� ������ ����, �� �Ʒ� �������� �ٸ������� �����ؾ���.
    {
        BookHead_State.state = BookHeadAI.State.RESPAWN;
        yield return new WaitForSeconds(3.5f); //3.5�� �ڿ� ��ü ����
        SpawnManager.S_instance.BookHeadRespawn(SpawnPoint, gameObject);
        BookHead_Animator.SetBool(ResetBool, true);
        gameObject.SetActive(false);
    }

    private void ShowEffect(Vector3 hitPos, Vector3 hitNormal) //���� ���� ��ġ ���� ������ ����ĳ��Ʈ�� ������ ����, �̸� ���� �°� ����Ʈ ȿ�� ����
    {

    }
}
