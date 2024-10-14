using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour, IItem
{
    private ItemData FlashLightData;
    private GameObject Player;
    private Inventory inventory;

    [SerializeField] Light[] flashlights;
    [SerializeField] BoxCollider flashCollider;
    [SerializeField] Transform Enemy_transform;
    [SerializeField] Transform FlashLight_transform;
    [SerializeField] Image flash_Battery;

    [SerializeField] private bool isOn = false;

    private float timer;
    private float prevTime;

    private readonly float MaxChaging = 60f;
    private readonly float Delay = 0.2f;

    void Awake()
    {
        timer = 20f;

        FlashLightData = new ItemData(gameObject, null, 0, "FlashLightImg_Group");

        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();

        FlashLight_transform = transform;
        
        flashlights = GetComponentsInChildren<Light>();
        flashCollider = GetComponentInChildren<BoxCollider>();
        flash_Battery= GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(0).GetComponent<Image>();

        flash_Battery.fillAmount = timer / MaxChaging;

        prevTime = Time.time;
    }
    private void OnEnable()
    {
        BatteryState();
    }

    private void BatteryState()
    {
        if (timer >= 60)
            inventory.CanGetBattery = false;
        else
            inventory.CanGetBattery = true;
    }

    void Update()
    {       
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
        BatteryState();
    }

    public void CatchItem()
    {
        inventory.GetItem(FlashLightData);
    }

    public void Use()
    {
        if(timer > 0f && Time.time - prevTime > Delay)
        {
            ToggleFlashlights();
            prevTime = Time.time;
        }
    }

    public void ItemUIOn()
    {
        if (inventory.CanGetItem)
            InGameUIManager.instance.OnPlayerUI_Text("������ [E]");
        else
            InGameUIManager.instance.OnPlayerUI_Text("�κ��丮�� �� á���ϴ�.");
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(FlashLightData.Item_Icon, FlashLightData.ItemUIGroup);
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
        while (isOn) // �÷��ö���Ʈ�� ���� ���� ���� ����
        {
            flash_Battery.fillAmount = timer /MaxChaging;
            yield return new WaitForSeconds(0.5f); // 1�ʸ��� ���͸� ����
            timer--;

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
    }
    public void CollectBattery()
    {
        timer += 40f; 
        flash_Battery.fillAmount = timer / MaxChaging; 
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

    private void OnDisable()
    {
        inventory.CanGetBattery = false;
    }
}