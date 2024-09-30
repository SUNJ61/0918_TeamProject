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
            isFlashing = true; // 충돌 시작 시 플래시 상태 설정
            timer = 0f; // 타이머 초기화
          
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFlashing && timer < 2f)
        {
            timer += Time.deltaTime; // 타이머 증가
           
            if (timer >= 2f)
            {
                timer = 5f; // 최대 5초로 제한
                isFlashing = false; // 플래시 상태 종료
                StartCoroutine(DontMove());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            isFlashing = false; // 충돌 종료 시 플래시 상태 해제
            timer = 0f; // 타이머 초기화
           
        }
    }

    IEnumerator DontMove()
    {
        if (enemy.Killplayer == false)
        {
            Enemyagent.isStopped = true; // 이동 멈춤
            Enemyagent.speed = 0;
            Enemyanimator.SetTrigger("Flash"); // 애니메이션 트리거
            yield return new WaitForSeconds(3f); // 3초 대기
            Enemyagent.speed = 5;
            Enemyagent.isStopped = false; // 다시 이동 시작
        }
       
    }
}
