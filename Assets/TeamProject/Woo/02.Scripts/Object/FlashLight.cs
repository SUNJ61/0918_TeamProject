using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    [SerializeField] Light[] flashlights;
    [SerializeField] CapsuleCollider flashCollider;
    [SerializeField] Transform Enemy_transform;
    [SerializeField] Transform FlashLight_transform;
    [SerializeField] Image flash_Battery;
    [SerializeField] Image flash_Battery_BG;
    [SerializeField] Image Aim;
    [SerializeField] CamerRay camerRay;
    bool isOn = false;
    float timer = 60f;

    void Start()
    {

        FlashLight_transform = transform;
        
        flashlights = GetComponentsInChildren<Light>();
        flashCollider = GetComponentInChildren<CapsuleCollider>();
        flash_Battery= GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(1).GetComponent<Image>();
        flash_Battery_BG = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        Aim = GameObject.Find("PlayerUi").transform.GetChild(0).GetComponent<Image>();
        camerRay = Camera.main.GetComponent<CamerRay>();
        
    }

    void Update()
    {
        if (isOn)
        {
            // Demon 게임 오브젝트 찾기
            GameObject demon = GameObject.Find("Demon_M(Clone)");
            if (demon != null)
            {
                var distance = Vector3.Distance(FlashLight_transform.position, demon.transform.position);
                

                if (distance < 7)
                {
                    if (!IsInvoking("ToggleFlashCollider"))
                    {
                        InvokeRepeating("ToggleFlashCollider", 0f, 0.2f); // 0.2초 간격으로 ToggleFlashCollider 호출
                    }
                }
                else if (distance < 3)
                {
                    if (!IsInvoking("ToggleFlashCollider"))
                    {
                        InvokeRepeating("ToggleFlashCollider", 0f, 0.5f); // 0.5초 간격으로 ToggleFlashCollider 호출
                    }
                }
                else
                {
                    if (IsInvoking("ToggleFlashCollider"))
                    {
                        CancelInvoke("ToggleFlashCollider"); // 거리가 멀어지면 반복 호출 중지
                        foreach (var flashlight in flashlights)
                        {
                            flashlight.enabled = isOn; // 플래시라이트 상태 설정
                        }
                    }
                }
            }
            else
            {
                // Demon이 없을 때 ToggleFlashCollider 호출 중지
                if (IsInvoking("ToggleFlashCollider"))
                {
                    CancelInvoke("ToggleFlashCollider");
                }
            }
        }




        if (Input.GetKeyDown(KeyCode.E))
        {
            if (camerRay.isTake)
            {
                if (transform.IsChildOf(GameObject.Find("HandPos").transform))
                {
                    ToggleFlashlights();
                }
            }
          
        }
       

    }

    private void ToggleFlashlights()
    {
        isOn = !isOn; // 상태 전환
        foreach (var flashlight in flashlights)
        {
            flashlight.enabled = isOn; // 플래시라이트 상태 설정
        }
        flashCollider.enabled = isOn;

        if (isOn)
        {
            StartCoroutine(BattertCount());
        }
        else
        {
            StopCoroutine(BattertCount());
        }
    }

    IEnumerator BattertCount()
    {
        while (timer > 0 && isOn) // 플래시라이트가 켜져 있을 때만 실행
        {
            flash_Battery.fillAmount = timer / 60f;
            yield return new WaitForSeconds(0.5f); // 1초마다 배터리 감소
            timer--;
        }
        if (timer <= 0)
        {
            isOn = false;
            foreach (var flashlight in flashlights)
            {
                flashlight.enabled = false; // 플래시라이트 꺼짐
            }
            flashCollider.enabled = false;
            flash_Battery.fillAmount = 0; // 배터리가 다 소모되면 UI 업데이트
        }
    }
    public void CollectBattery(IBattery battery)
    {
        timer = Mathf.Min(timer + battery.ChargeBattery, 60f); 
        flash_Battery.fillAmount = timer / 60f; 
    }

    private void ToggleFlashCollider()
    {
        if (isOn == true)
        {
            foreach (var flashlight in flashlights)
            {
                flashlight.enabled = !flashlight.enabled;
            }
        }
        

    }
    void KillPlayer()
    {
        isOn = false;
        foreach (var flashlight in flashlights)
        {
            flashlight.enabled = false;
        }
        flashCollider.enabled = false;

        flash_Battery.enabled = false;
        flash_Battery_BG.enabled = false;
        MeshRenderer meshRenderer_flash = GetComponent<MeshRenderer>();
        meshRenderer_flash.enabled = false;
    }
}
