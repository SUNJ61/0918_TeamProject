using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonMove : MonoBehaviour
{
    private Transform PlayerPos;
    private Transform DemonPos;
    private NavMeshAgent Demon_agent;
    private Animator Demon_animator;
    private CapsuleCollider Demon_cap;
    private ParticleSystem particle_somoke;
    private Rigidbody Demon_Rb;

    private AudioClip DemonDie_SFX;
    private AudioClip SpawnDemon_SFX;
    private AudioClip Demon_Sound_SFX;

    private float Damping;
    private float TraceSpeed = 5.0f;

    public int Demon_Counter = 0;

    private bool isTrace;
    public bool IsTrace
    {
        get { return isTrace; }
        set 
        {
           isTrace = value;
            if (isTrace)
                PlayerTrace();
        }
    }

    private bool isIdle;
    public bool IsIdle
    {
        get { return isIdle; }
        set 
        {
            isIdle = value;
            if(isIdle)
            {
                DemonIdle();
            }
        }
    }

    private void Awake()
    {
        DemonPos = transform;
        PlayerPos = GameObject.Find("Player").transform;
        Demon_agent = GetComponent<NavMeshAgent>();
        Demon_animator = GetComponent<Animator>();
        Demon_cap = GetComponent<CapsuleCollider>();
        Demon_Rb = GetComponent<Rigidbody>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();

        SpawnDemon_SFX = Resources.Load<AudioClip>("Sound/Demon/Demon_SpwanSound");
        Demon_Sound_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonBgSound");
        DemonDie_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonDie");
    }

    private void Update()
    {
        if(Demon_agent.isStopped == false && !GameManager.G_instance.isGameover)
        {
            Quaternion rot = Quaternion.LookRotation(PlayerPos.position - DemonPos.position);
            DemonPos.rotation = Quaternion.Lerp(DemonPos.rotation, rot, Time.deltaTime * Damping);
        }
    }

    private void DemonIdle()
    {
        Demon_animator.SetBool("IsIdle", true);
        Demon_animator.SetBool("IsRun", false);
        Demon_agent.isStopped = true;
    }

    private void PlayerTrace()
    {
        IsIdle = false;
        Demon_animator.SetBool("IsIdle", false);
        Demon_animator.SetBool("IsRun", true);
        Demon_agent.isStopped = false;
        Demon_agent.speed = TraceSpeed;
        Demon_agent.destination = PlayerPos.position;
        Damping = 5.0f;
    }

    public void DemonStop()
    {
        Demon_agent.isStopped = true;
    }

    public void PlayerDie()
    {
        Demon_animator.SetBool("IsIdle", true);
        Demon_animator.SetBool("IsRun", false);
        Demon_agent.isStopped = true;

        InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
        InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
    }

    public void FalseDemon()
    {
        StartCoroutine(FalseDemonRoutine());
    }

    public void TrueDemon()
    {
        Demon_agent.isStopped = true;
        Demon_animator.SetTrigger("Spawn");

        Demon_Sound_SFX.name = $"DemonBgSound_{Demon_Counter}";
        InGameSoundManager.instance.ActiveSound(gameObject, SpawnDemon_SFX, 15, true, false, false, 1);
        InGameSoundManager.instance.ActiveSound(gameObject, Demon_Sound_SFX, 5, false, true, true, 1);
    }

    IEnumerator FalseDemonRoutine()
    {
        particle_somoke.Stop();
        Demon_agent.isStopped = true;
        Demon_Rb.isKinematic = true;
        Demon_animator.SetTrigger("Out");
        Demon_cap.enabled = false;
        InGameSoundManager.instance.ActiveSound(gameObject, DemonDie_SFX, 7, true, false, false, 1, 2f);
        InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
        InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            Demon_Rb.isKinematic = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            Demon_Rb.isKinematic = false;
    }
}
