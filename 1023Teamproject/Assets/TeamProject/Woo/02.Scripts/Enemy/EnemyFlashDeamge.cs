using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlashDamage : MonoBehaviour
{
    public int Demon_Counter = 0;
    float timer;
    bool isSoundPlay;
    [SerializeField] bool isFlashing;

    NavMeshAgent Enemyagent;
    Animator Enemyanimator;
    [SerializeField] Enemy enemy;
    [SerializeField] ParticleSystem particle_somoke;
    [SerializeField] CapsuleCollider Demon_cap;
    [SerializeField] AudioClip Demon_Steam;

    private void Start()
    {
        Demon_Steam = Resources.Load<AudioClip>("Sound/Demon/Demon_Steam");

        Demon_cap = GetComponent<CapsuleCollider>();
        timer = 0f;
        isFlashing = false;
        Enemyagent = GetComponent<NavMeshAgent>();
        Enemyanimator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();
        particle_somoke.Stop();
    }
    private void OnEnable()
    {
        if (Enemyagent == null)
        {
            return;
        }
        else
        {
            timer = 0;
            Enemyagent.isStopped = false;
            Enemyagent.speed = 5;
            Demon_cap.enabled = true;
            particle_somoke.Stop();
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            isFlashing = true; // 충돌 시작 시 플래시 상태 설정
            print("충돌 시작");
            if (timer > 0.1f)
            {
                particle_somoke.Play();
                if (isFlashing && !isSoundPlay)
                {
                    Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                    InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 5, true, true, true, 1);
                    isSoundPlay = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FlashCol") && isFlashing && timer < 3f)
        {

            timer += Time.deltaTime; // 타이머 증가

            if (timer >= 3f)
            {
                if (isSoundPlay)
                {

                    InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                    InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                    isSoundPlay = false;
                }
                timer = 0f;
                isFlashing = false;
                StartCoroutine(DontMove());
            }
        }
    }


    IEnumerator DontMove()
    {
        if (enemy.Killplayer == false)
        {
            print("코루틴 호출");
            Demon_cap.enabled = false;
            particle_somoke.Stop();
            Enemyagent.isStopped = true; // 이동 멈춤
            Enemyagent.speed = 0;
            Enemyanimator.SetTrigger("Flash"); // 애니메이션 트리거

            yield return new WaitForSeconds(4.5f); // 3초 대기

            Enemyagent.speed = 5;
            Enemyagent.isStopped = false; // 다시 이동 시작
            isFlashing = false; // 플래시 상태 종료
            Demon_cap.enabled = true;
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            if (isSoundPlay && InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
            {
                InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
            }
            isFlashing = false; // 플래시 상태 종료
            timer = 0f; // 타이머 초기화
            particle_somoke.Stop();
        }
    }

}
