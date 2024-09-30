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
        // Demon 오브젝트를 인스턴스화
        GameObject Demonepos = GameObject.Find("Demon_M(Clone)").gameObject;
        if(Demonepos != null )
        {
            _cam.Follow = Demonepos.transform;
            _cam.LookAt = Demonepos.transform;
            //var transposer = _cam.GetCinemachineComponent<CinemachineFramingTransposer>();
            //transposer.m_TrackedObjectOffset = followOffset; // Follow Offset 적용 
            //var composer = _cam.GetCinemachineComponent<CinemachineComposer>();
            //if (composer != null)
            //{
            //    composer.m_TrackedObjectOffset = aimOffset; // Aim Offset 적용
            //}// 오류이슈로 일단 보류

        }
        else if(Demonepos == null)
        {
            _cam.Follow = null;
            _cam.LookAt = null;
        }
       

    }





}
