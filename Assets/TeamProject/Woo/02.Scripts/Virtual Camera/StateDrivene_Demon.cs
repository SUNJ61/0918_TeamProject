using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDrivene_Demon : MonoBehaviour
{
    [SerializeField] CinemachineStateDrivenCamera _cam;
    [SerializeField] Transform Demon_tr;
    [SerializeField] Animator Demon_ani;

    void Start()
    {
        _cam = GetComponent<CinemachineStateDrivenCamera>();
        SpawnDemon();
    }

    public void SpawnDemon()
    {
       
        // Resources.Load�� ������Ʈ�� �ε��ϰ� �ν��Ͻ�ȭ
        GameObject demonPrefab = Resources.Load<GameObject>("Demon_M");

        if (demonPrefab != null)
        {
            // �ν��Ͻ�ȭ�Ͽ� ���� �߰�
            GameObject demonObject = Instantiate(demonPrefab, transform.position, Quaternion.identity);
            Demon_tr = demonObject.transform; // Transform �Ҵ�
            Demon_ani = demonObject.GetComponent<Animator>(); // Animator ��������
            // ī�޶��� Animated Target ����
            _cam.m_AnimatedTarget = Demon_ani;
        }
        else
        {
            Debug.LogError("Demon ������Ʈ�� ������ �� �����ϴ�.");
        }


    }
}
