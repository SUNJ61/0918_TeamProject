using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DemonDamage : MonoBehaviour
{
    public int Demon_Counter = 0;
    float timer;
    bool isSoundPlay;
    bool isFlashing;

    NavMeshAgent Demon_agent;
    Animator Demon_animator;
    ParticleSystem particle_somoke;
    CapsuleCollider Demon_cap;
    AudioClip Demon_Steam;
    DemonAI demonAI;

    private void Awake()
    {
        Demon_Steam = Resources.Load<AudioClip>("Sound/Demon/Demon_Steam");

        Demon_cap = GetComponent<CapsuleCollider>();
        Demon_agent = GetComponent<NavMeshAgent>();
        Demon_animator = GetComponent<Animator>();
        demonAI = GetComponent<DemonAI>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();

        isFlashing = false;
        isSoundPlay = false;
    }

    private void OnEnable()
    {
            timer = 0;
            Demon_cap.enabled = true;
            particle_somoke.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            print("손전등 맞음");
            if (!isFlashing)
            {
                isFlashing = true;
                particle_somoke.Play();
                StartCoroutine(TimerUpdate());

                if(!isSoundPlay)
                {
                    Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                    InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 5, true, true, true, 1);
                    isSoundPlay = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashCol"))
        {
            if (isFlashing)
            {
                isFlashing = false;
                particle_somoke.Stop();
                timer = 0f;

                if(isSoundPlay)
                {
                    InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                    InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                    isSoundPlay = false;
                }
            }
        }
    }

    IEnumerator TimerUpdate()
    {
        while (timer <= 3.0f)
        {
            if (!isFlashing) //손전등이 괴물한테서 떨어지면
                yield break; //코루틴 탈출

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;

            if (timer >= 3.0f) //3초이상 플래쉬를 맞으면
            {
                Demon_cap.enabled = false;
                if (isSoundPlay)
                {
                    InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                    InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                    isSoundPlay = false;
                }
                demonAI.Demon_isDamage = true;
                Demon_agent.isStopped = true;
                particle_somoke.Stop();
                Demon_animator.SetTrigger("Flash"); // 애니메이션 트리거

                yield return new WaitForSeconds(4.5f); // 4.5초 스턴

                timer = 0f;
                demonAI.Demon_isDamage = false;
                isFlashing = false;
                Demon_cap.enabled = true;

                yield break; //코루틴 탈출
            }
        }
    }

    private void OffFlashLight()
    {
        if (isFlashing)
        {
            isFlashing = false;
            particle_somoke.Stop();
            timer = 0f;

            if (isSoundPlay)
            {
                InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                isSoundPlay = false;
            }
        }
    }
}
