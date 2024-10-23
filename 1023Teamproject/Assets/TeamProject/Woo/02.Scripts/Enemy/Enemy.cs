using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class Enemy : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public string PlayTag = "Player";
    public Transform PlayerPos;
    public Transform tr;
    [SerializeField] Light DeadSceneLight;
    [SerializeField] PlayableDirector director;
    [Tooltip("공격사거리와 파악위치 사거리")]
    float distance = 20;
    float attackside = 2.0f;
    public int Demon_Counter = 0;
    public bool Killplayer = false;
    private bool isDead = false;
    float timer = 0;
    [SerializeField] CinemachineStateDrivenCamera State_Demon;
    [SerializeField] CinemachineVirtualCamera VirtualCamera_Demon;
    [SerializeField] CapsuleCollider Demon_cap;
    [SerializeField] ParticleSystem particle_somoke;
    [SerializeField] AudioClip DemonDie_SFX;
    [SerializeField] AudioClip DemonAttack_SFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        PlayTag = GameObject.Find(PlayTag).tag;
        tr = GetComponent<Transform>();
        PlayerPos = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine(StartFollowingPlayer());
        DeadSceneLight = transform.GetChild(2).gameObject.GetComponent<Light>();
        DeadSceneLight.enabled = false;
        director = GameObject.Find("TimeLine_Demon").gameObject.GetComponent<PlayableDirector>();
        Demon_cap = GetComponent<CapsuleCollider>();
        particle_somoke = transform.GetChild(5).GetComponent<ParticleSystem>();
        DemonDie_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonDie");
        DemonAttack_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonAttackSound");
    }
    private void OnEnable()
    {
        timer = 0;
        StartCoroutine(StartFollowingPlayer());
        InvokeRepeating(nameof(UpdateTimer), 0f, 1f); // 1초마다 UpdateTimer 호출
        rb.isKinematic = false;
        isDead = false;
    }

    private void UpdateTimer()
    {
        if (!GameManager.G_instance.isGameover)
        {
            timer += 1f;
            print(timer);
            if (timer >= 30)
            {
                InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
                InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
                InGameSoundManager.instance.ActiveSound(gameObject, DemonDie_SFX, 7, true, false, false, 1, 2f);
                
                timer = 30;
                
                StartCoroutine(FalseDemon());
                
            }
        }      
    }

    private void FollowPlayertoAttack()
    {
        if (!GameManager.G_instance.isGameover)
        {
            var Distance = Vector3.Distance(PlayerPos.transform.position, tr.transform.position);
            if (Distance <= attackside)
            {

                animator.SetBool("IsRun", false);
                animator.SetBool("Attack", true);

                agent.isStopped = true;
                Vector3 Playerpos = (PlayerPos.position - transform.position).normalized;
                Quaternion rot = Quaternion.LookRotation(Playerpos);
            }
            else if (Distance <= distance)
            {
                distance = 50f;
                agent.destination = PlayerPos.transform.position;
                animator.SetBool("IsRun", true);
                animator.SetBool("Attack", false);
                agent.isStopped = false;
                Vector3 Playerpos = (PlayerPos.position - transform.position).normalized;
                Quaternion rot = Quaternion.LookRotation(Playerpos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3F); 
            }
        }

        if (GameManager.G_instance.isGameover)
        {
            if (!isDead) // 죽지 않은 상태일 때만 게임 오버 처리
            {
                StopCoroutine(StartFollowingPlayer());
                animator.speed = 0;
                rb.isKinematic = true;
                rb.freezeRotation = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                agent.isStopped = true;
                agent.speed = 0;
            }
        }

        if (GameManager.G_instance.AllStop)
        {
            StartCoroutine(AllStop());
        }
    }
    IEnumerator AllStop()
    {
        agent.speed = 0;
        agent.isStopped = true;

        yield return new WaitForSeconds(5f);
        agent.isStopped = false;
        agent.speed = 5f;
    }
    IEnumerator FalseDemon()
    {
        StopCoroutine(StartFollowingPlayer());

        particle_somoke.Stop();
        agent.isStopped = true;
        agent.speed = 0;
        rb.isKinematic = true;
        animator.SetTrigger("Out");
        Demon_cap.enabled = false;
        CancelInvoke(nameof(UpdateTimer));
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        timer = 0;
    }
    IEnumerator StartFollowingPlayer()
    { 
        while (true) // 무한 루프를 통해 계속 플레이어를 추적
        {
            FollowPlayertoAttack();
            yield return null; // 다음 프레임까지 대기
        }
    }
    void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        animator.speed = 1;
        InGameSoundManager.instance.EditSoundBox($"DemonBgSound_{Demon_Counter}", false);
        InGameSoundManager.instance.Data.Remove($"DemonBgSound_{Demon_Counter}");
        particle_somoke.Stop();
        Killplayer = true;
        DeadSceneLight.enabled = true;
        StopCoroutine("FalseDemon()");
        StopCoroutine("StartFollowingPlayer()");
        agent.isStopped = true; // 이동 멈춤
        agent.speed = 0;
        animator.SetTrigger("Kill");
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        director.Play();
        DeadSceneLight.enabled = true;
        State_Demon.Priority = 20;
        VirtualCamera_Demon.Priority = 20;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateTimer));
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            rb.isKinematic = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            rb.isKinematic = false;
    }
    public void Attack()
    {
        InGameSoundManager.instance.ActiveSound(gameObject, DemonAttack_SFX, 5, true, false, false, 1);
    }
}
