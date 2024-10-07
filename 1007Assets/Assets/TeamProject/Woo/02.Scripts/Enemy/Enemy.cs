using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    float distance = 15f;
    float attackside = 3f;
    int AttackCombo;
    public bool Killplayer = false;
    float timer = 0;
    [SerializeField] CinemachineStateDrivenCamera State_Demon;
    [SerializeField] CinemachineVirtualCamera VirtualCamera_Demon;
    [SerializeField] CapsuleCollider Demon_cap;

    void Start()
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
    }
    private void Update()
    {
        if (!GameManager.G_instance.isGameover)
        {
            timer += Time.deltaTime;
            print(timer);
            if (timer >= 30)
            {
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

                AttackCombo++;
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
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3F); // rotationSpeed는 회전 속도
            }
        }
        

        if (GameManager.G_instance.isGameover)
        {

            agent.enabled = false;
            animator.enabled = false;
            StopCoroutine(StartFollowingPlayer());
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }
    IEnumerator FalseDemon()
    {
        agent.isStopped= true;
        agent.speed = 0;
        animator.SetTrigger("Out");
        Demon_cap.enabled = false;
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
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
        Killplayer = true;
        DeadSceneLight.enabled = true;
        StopAllCoroutines();
        agent.isStopped = true; // 이동 멈춤
        agent.speed = 0;
        animator.SetTrigger("Kill");
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY;
        director.Play();
        DeadSceneLight.enabled = true;
        State_Demon.Priority = 20;
        VirtualCamera_Demon.Priority = 20;
    }
}
