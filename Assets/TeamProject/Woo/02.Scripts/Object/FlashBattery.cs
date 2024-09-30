using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBattery : MonoBehaviour
{
    [SerializeField] Image Battery; // 배터리 이미지
    public FlashLight flashLight;
    private float timer = 60f; // 초기 타이머 값
    private bool isActive = false; // 활성화 상태

    private void OnEnable()
    {
        Battery= GetComponent<Image>(); 
        isActive = true; // 활성화 상태로 변경
        StartCoroutine(BatteryCountdown()); // 카운트다운 시작
    }


    IEnumerator BatteryCountdown()
    {
        while (timer > 0)
        {
            // 배터리 이미지의 fillAmount를 조정
            Battery.fillAmount = timer / 60f;
            yield return new WaitForSeconds(0.1f); // 1초 대기
            timer--; // 타이머 감소

        }
        if (timer == 0)
        {
          
        }


    }

 
}
