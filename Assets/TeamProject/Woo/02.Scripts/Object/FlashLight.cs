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

    [SerializeField] private string ParentName;

    bool isOn = false;

    float timer = 60f;

    void Start()
    {
        //timer = 0f;

        FlashLight_transform = transform;
        
        flashlights = GetComponentsInChildren<Light>();
        flashCollider = GetComponentInChildren<CapsuleCollider>();
        flash_Battery= GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(1).GetComponent<Image>();
        flash_Battery_BG = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        Aim = GameObject.Find("PlayerUi").transform.GetChild(0).GetComponent<Image>();
        camerRay = GameObject.Find("Player").transform.GetChild(0).GetComponent<CamerRay>();

        ParentName = transform.parent.parent.parent.name;
    }

    void Update()
    {
        ParentName = transform.parent.parent.parent.name; //�÷��� ��ġ ���� ��� ������Ʈ

        if (ParentName == "Player") //������ ���� �ȿ� ���� �� (���� �տ� ����� �� ���� ������Ʈ �ʿ�)
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();
            
            if(timer > 0)
                item.IsFlash = true;
            if(timer <= 0)
                item.IsFlash = false;
        }
            

        if (isOn) //�տ� ����� ���� �����ϴ� ���� �ʿ�.
        {
            // Demon ���� ������Ʈ ã��
            GameObject demon = GameObject.Find("Demon_M");
            if (demon != null)
            {
                var distance = Vector3.Distance(FlashLight_transform.position, demon.transform.position);
                

                if (distance < 7)
                {
                    if (!IsInvoking("ToggleFlashCollider"))
                    {
                        InvokeRepeating("ToggleFlashCollider", 0f, 0.2f); // 0.2�� �������� ToggleFlashCollider ȣ��
                    }
                }
                else if (distance < 3)
                {
                    if (!IsInvoking("ToggleFlashCollider"))
                    {
                        InvokeRepeating("ToggleFlashCollider", 0f, 0.5f); // 0.5�� �������� ToggleFlashCollider ȣ��
                    }
                }
                else
                {
                    if (IsInvoking("ToggleFlashCollider"))
                    {
                        CancelInvoke("ToggleFlashCollider"); // �Ÿ��� �־����� �ݺ� ȣ�� ����
                        foreach (var flashlight in flashlights)
                        {
                            flashlight.enabled = isOn; // �÷��ö���Ʈ ���� ����
                        }
                    }
                }
            }
            else
            {
                // Demon�� ���� �� ToggleFlashCollider ȣ�� ����
                if (IsInvoking("ToggleFlashCollider"))
                {
                    CancelInvoke("ToggleFlashCollider");
                }
            }
        }
    }

    private void ToggleFlashlights()
    {
        isOn = !isOn; // ���� ��ȯ
        foreach (var flashlight in flashlights)
        {
            flashlight.enabled = isOn; // �÷��ö���Ʈ ���� ����
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
        while (timer > 0 && isOn) // �÷��ö���Ʈ�� ���� ���� ���� ����
        {
            flash_Battery.fillAmount = timer / 60f;
            yield return new WaitForSeconds(0.5f); // 1�ʸ��� ���͸� ����
            timer--;
        }
        if (timer <= 0)
        {
            isOn = false;
            foreach (var flashlight in flashlights)
            {
                flashlight.enabled = false; // �÷��ö���Ʈ ����
            }
            flashCollider.enabled = false;
            flash_Battery.fillAmount = 0; // ���͸��� �� �Ҹ�Ǹ� UI ������Ʈ
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
