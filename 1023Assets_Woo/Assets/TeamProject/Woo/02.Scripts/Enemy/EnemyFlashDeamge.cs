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
            print("�浹 ����");
            isFlashing = true; // �浹 ���� �� �÷��� ���� ����
            particle_somoke.Play();

            // ���� ��� ����
            if (!isSoundPlay)
            {
                if (Demon_Steam != null)
                {
                    Demon_Steam.name = $"Demon_Steam_{Demon_Counter}";
                    InGameSoundManager.instance.ActiveSound(gameObject, Demon_Steam, 5, true, true, true, 1);
                    Debug.Log($"ActiveSound ȣ��: ���� ������Ʈ = {gameObject.name}, ���� Ŭ�� = {Demon_Steam.name}, ���� = 5");
                    isSoundPlay = true; // ���尡 ��� ������ ǥ��
                }
                else
                {
                    Debug.LogError("Demon_Steam�� null�Դϴ�. ���带 ����� �� �����ϴ�.");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FlashCol") && isFlashing && timer < 3f)
        {

            timer += Time.deltaTime; // Ÿ�̸� ����

            if (timer >= 3f)
            {
                if (isSoundPlay)
                {
                    print("���� ��Ÿ��");
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
            print("�ڷ�ƾ ȣ��");
            Demon_cap.enabled = false; // �浹 ��Ȱ��ȭ ���� ����
            particle_somoke.Stop();
            Enemyagent.isStopped = true; // �̵� ����
            Enemyagent.speed = 0;
            Enemyanimator.SetTrigger("Flash"); // �ִϸ��̼� Ʈ����

            yield return new WaitForSeconds(4.5f); // ���
            Demon_cap.enabled = true; // �浹 ��Ȱ��ȭ ���� ����
            Enemyagent.speed = 5;
            Enemyagent.isStopped = false; // �ٽ� �̵� ����
                                          // Demon_cap.enabled = true; // �ʿ��� ��� �ٽ� Ȱ��ȭ
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FlashCol"))
        {
            print("�浹����");
            if (isSoundPlay && InGameSoundManager.instance.Data.ContainsKey($"Demon_Steam_{Demon_Counter}"))
            {
                InGameSoundManager.instance.EditSoundBox($"Demon_Steam_{Demon_Counter}", false);
                Debug.Log($"EditSoundBox ȣ��: {Demon_Steam}_{Demon_Counter} ����");
                InGameSoundManager.instance.Data.Remove($"Demon_Steam_{Demon_Counter}");
                Debug.Log($"Data���� {Demon_Steam}_{Demon_Counter} ����");

                // ���� �ʱ�ȭ
                isFlashing = false; // �÷��� ���� ����
                timer = 0f; // Ÿ�̸� �ʱ�ȭ
                particle_somoke.Stop();
                isSoundPlay = false; // ���� ���� �ʱ�ȭ
            }
        }
    }

   


}
