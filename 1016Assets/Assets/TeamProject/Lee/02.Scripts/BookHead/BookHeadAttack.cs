using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BookHeadAttack : MonoBehaviour
{
    private GameObject Player;
    private Transform Player_Transform;
    private Transform BookHead_Transform;
    private Animator BookHead_animator;
    private NavMeshAgent BookHead_agent;
    private BoxCollider BookHead_hitbox;
    private PlayableDirector BookHead_TimeLine;
    private Light KillLight;
    private CinemachineStateDrivenCamera KillActionCam_BookHead;

    private Quaternion rot;

    private float Damping = 10.0f;

    private readonly int hashMovement = Animator.StringToHash("Movement");
    private readonly int hashAttack = Animator.StringToHash("Attack");

    private readonly string playerTag = "Player";

    private bool _isAttack;
    public bool isAttack
    {
        get { return _isAttack; }
        set
        {
            _isAttack = value;
            if(_isAttack == true)
            {
                StartCoroutine(BiteAttack());
            }
        }
    }

    private void Awake()
    {
        Player = GameObject.FindWithTag(playerTag);
        Player_Transform = Player.GetComponent<Transform>();
        BookHead_Transform = transform;
        BookHead_animator = GetComponent<Animator>();
        BookHead_agent = GetComponent<NavMeshAgent>();
        BookHead_hitbox = transform.GetChild(1).GetComponent<BoxCollider>();
        BookHead_TimeLine = transform.GetChild(3).GetComponent<PlayableDirector>();
        KillLight = transform.GetChild(4).GetComponent<Light>();
        KillActionCam_BookHead = transform.GetChild(2).GetChild(0).GetComponent<CinemachineStateDrivenCamera>();
    }

    private void Update()
    {
        if(isAttack == true)
        {
            rot = Quaternion.LookRotation(Player_Transform.position - BookHead_Transform.position);
            BookHead_Transform.rotation = Quaternion.Lerp(BookHead_Transform.rotation, rot, Time.deltaTime * Damping);
        }
    }

    IEnumerator BiteAttack()
    {
        BookHead_agent.isStopped = true;
        BookHead_animator.SetFloat(hashMovement, 0.0f);

        yield return new WaitForSeconds(0.1f);

        BookHead_animator.SetBool(hashAttack, isAttack);

        yield return new WaitForSeconds(1.6f);

        isAttack = false;
        BookHead_animator.SetBool(hashAttack, isAttack);
        BookHead_agent.isStopped = false;
    }

    private void KillPlayer() //북헤드 정지 기능 만들어야함.
    {
        BookHead_agent.isStopped = true;
        BookHead_animator.SetTrigger("PlayerDie");
        BookHead_TimeLine.Play();
        KillLight.enabled = true;
        KillActionCam_BookHead.Priority = 20;
    }

    public void OnHitBox()
    {
        BookHead_hitbox.enabled = true;
        Invoke("OffHitBox", 0.02f);
    }
    public void OffHitBox()
    {
        BookHead_hitbox.enabled = false;
    }
}
