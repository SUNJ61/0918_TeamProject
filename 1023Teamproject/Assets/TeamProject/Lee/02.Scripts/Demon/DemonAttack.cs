using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class DemonAttack : MonoBehaviour
{
    private Transform DemonPos;
    private Transform PlayerPos;
    private NavMeshAgent Demon_agent;
    private Animator Demon_animator;
    private DemonAI demonAI;
    private ParticleSystem particle_somoke;

    private CinemachineStateDrivenCamera State_Demon;
    private CinemachineVirtualCamera VirtualCamera_Demon;
    private Light DeadSceneLight;
    private PlayableDirector Demon_TimeLine;

    private AudioClip DemonAttack_SFX;

    private Quaternion rot;

    private readonly float Damping = 10.0f;

    public int Demon_Counter = 0;

    private bool isAttack;
    public bool IsAttack
    {
        get { return isAttack; }
        set
        {
            isAttack = value;
            if (isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private void Awake()
    {
        PlayerPos = GameObject.Find("Player").transform;
        DemonPos = GetComponent<Transform>();
        Demon_agent = GetComponent<NavMeshAgent>();
        Demon_animator = GetComponent<Animator>();
        demonAI = GetComponent<DemonAI>();

        DeadSceneLight = transform.GetChild(2).gameObject.GetComponent<Light>();
        Demon_TimeLine = GameObject.Find("TimeLine_Demon").gameObject.GetComponent<PlayableDirector>();
        State_Demon = transform.GetChild(3).GetComponent<CinemachineStateDrivenCamera>();
        VirtualCamera_Demon = State_Demon.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();

        DemonAttack_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonAttackSound");

        DeadSceneLight.enabled = false;
    }

    private void Update()
    {
        if (IsAttack)
        {
            rot = Quaternion.LookRotation(PlayerPos.position - DemonPos.position);
            DemonPos.rotation = Quaternion.Lerp(DemonPos.rotation, rot, Time.deltaTime * Damping);
        }
    }

    IEnumerator Attack()
    {
        Demon_agent.isStopped = true;
        Demon_animator.SetBool("IsIdle", true);
        Demon_animator.SetBool("IsRun", false);

        yield return new WaitForSeconds(0.1f);

        Demon_animator.SetBool("IsIdle", false);
        Demon_animator.SetTrigger("Attack");
        InGameSoundManager.instance.ActiveSound(gameObject, DemonAttack_SFX, 5.0f, true, false, false, 1);

        IsAttack = false;
        Demon_agent.isStopped = false;
    }

    private void KillPlayer()
    {
        if (InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
        {
            InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
            InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
        }
        InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
        InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
        Demon_agent.isStopped = true;
        demonAI.Demon_isKill = true;
        particle_somoke.Stop();
        Demon_animator.SetTrigger("Kill");
        DeadSceneLight.enabled = true;
        Demon_TimeLine.Play();
        State_Demon.Priority = 20;
        VirtualCamera_Demon.Priority = 20;
    }
}
