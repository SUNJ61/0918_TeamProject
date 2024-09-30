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
       
        // Resources.Load로 오브젝트를 로드하고 인스턴스화
        GameObject demonPrefab = Resources.Load<GameObject>("Demon_M");

        if (demonPrefab != null)
        {
            // 인스턴스화하여 씬에 추가
            GameObject demonObject = Instantiate(demonPrefab, transform.position, Quaternion.identity);
            Demon_tr = demonObject.transform; // Transform 할당
            Demon_ani = demonObject.GetComponent<Animator>(); // Animator 가져오기
            // 카메라의 Animated Target 설정
            _cam.m_AnimatedTarget = Demon_ani;
        }
        else
        {
            Debug.LogError("Demon 오브젝트를 스폰할 수 없습니다.");
        }


    }
}
