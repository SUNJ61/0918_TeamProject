using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlashDamage : MonoBehaviour
{
    float timer;
    bool isFlashing;
    NavMeshAgent Enemyagent;
    Animator Enemyanimator;
    [SerializeField]Enemy enemy;

    private void Start()
    {
        timer = 0f;
        isFlashing = false;
        Enemyagent = GetComponent<NavMeshAgent>();
        Enemyanimator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            isFlashing = true; // �浹 ���� �� �÷��� ���� ����
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
          
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFlashing && timer < 2f)
        {
            timer += Time.deltaTime; // Ÿ�̸� ����
           
            if (timer >= 2f)
            {
                timer = 5f; // �ִ� 5�ʷ� ����
                isFlashing = false; // �÷��� ���� ����
                StartCoroutine(DontMove());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            isFlashing = false; // �浹 ���� �� �÷��� ���� ����
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
           
        }
    }

    IEnumerator DontMove()
    {
        if (enemy.Killplayer == false)
        {
            Enemyagent.isStopped = true; // �̵� ����
            Enemyagent.speed = 0;
            Enemyanimator.SetTrigger("Flash"); // �ִϸ��̼� Ʈ����
            yield return new WaitForSeconds(3f); // 3�� ���
            Enemyagent.speed = 5;
            Enemyagent.isStopped = false; // �ٽ� �̵� ����
        }
       
    }
}
