using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera_Demo : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _cam;
    bool spawn;
    Vector3 followOffset = new Vector3(0, 0.73f, 3.21f);
    Vector3 aimOffset = new Vector3(0, 1.77f, 0);

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();


    }

    void Update()
    {
        // Demon ������Ʈ�� �ν��Ͻ�ȭ
        GameObject Demonepos = GameObject.Find("Demon_M(Clone)").gameObject;
        if(Demonepos != null )
        {
            _cam.Follow = Demonepos.transform;
            _cam.LookAt = Demonepos.transform;
            //var transposer = _cam.GetCinemachineComponent<CinemachineFramingTransposer>();
            //transposer.m_TrackedObjectOffset = followOffset; // Follow Offset ���� 
            //var composer = _cam.GetCinemachineComponent<CinemachineComposer>();
            //if (composer != null)
            //{
            //    composer.m_TrackedObjectOffset = aimOffset; // Aim Offset ����
            //}// �����̽��� �ϴ� ����

        }
        else if(Demonepos == null)
        {
            _cam.Follow = null;
            _cam.LookAt = null;
        }
       

    }





}
