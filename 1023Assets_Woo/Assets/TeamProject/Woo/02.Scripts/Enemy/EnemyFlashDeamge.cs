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
        if (other.gameObject.CompareTag("FlashCol"))
        {
            print("충돌 감지");
            isFlashing = true; // 충돌 시작 시 플래시 상태 설정
            particle_somoke.Play();

            // 사운드 재생 로직
            if (!isSoundPlay)
            {
                if (Demon_Steam != null)
                {
                    Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                    InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 5, true, true, true, 1);
                    Debug.Log($"ActiveSound 호출: 게임 오브젝트 = {gameObject.name}, 사운드 클립 = {Demon_Steam.name}, 볼륨 = 5");
                    isSoundPlay = true; // 사운드가 재생 중임을 표시
                }
                else
                {
                    Debug.LogError("Demon_Steam이 null입니다. 사운드를 재생할 수 없습니다.");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FlashCol") && isFlashing && timer < 3f)
        {

            timer += Time.deltaTime; // 타이머 증가

            if (timer >= 3f)
            {
                if (isSoundPlay)
                {
                    print("사운드 쿨타임");
                    InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                    InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                    isSoundPlay = false;
                    timer = 0f;
                    isFlashing = false;
                    StartCoroutine(DontMove());
                }
                
            }
        }
    }


    IEnumerator DontMove()
    {
        if (enemy.Killplayer == false)
        {
            print("코루틴 호출");
            Demon_cap.enabled = false; // 충돌 비활성화 하지 않음
            particle_somoke.Stop();
            Enemyagent.isStopped = true; // 이동 멈춤
            Enemyagent.speed = 0;
            Enemyanimator.SetTrigger("Flash"); // 애니메이션 트리거

            yield return new WaitForSeconds(4.5f); // 대기
            Demon_cap.enabled = true; // 충돌 비활성화 하지 않음
            Enemyagent.speed = 5;
            Enemyagent.isStopped = false; // 다시 이동 시작
                                          // Demon_cap.enabled = true; // 필요할 경우 다시 활성화
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FlashCol"))
        {
            print("충돌해제");
            if (isSoundPlay && InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
            {
                InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                Debug.Log($"EditSoundBox 호출: {Demon_Steam}_{Demon_Counter} 종료");
                InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                Debug.Log($"Data에서 {Demon_Steam}_{Demon_Counter} 제거");

                // 상태 초기화
                isFlashing = false; // 플래시 상태 종료
                timer = 0f; // 타이머 초기화
                particle_somoke.Stop();
                isSoundPlay = false; // 사운드 상태 초기화
            }
        }
    }

   


}
