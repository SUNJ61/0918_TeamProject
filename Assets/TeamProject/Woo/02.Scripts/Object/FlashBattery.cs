using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBattery : MonoBehaviour
{
    [SerializeField] Image Battery; // ���͸� �̹���
    public FlashLight flashLight;
    private float timer = 60f; // �ʱ� Ÿ�̸� ��
    private bool isActive = false; // Ȱ��ȭ ����

    private void OnEnable()
    {
        Battery= GetComponent<Image>(); 
        isActive = true; // Ȱ��ȭ ���·� ����
        StartCoroutine(BatteryCountdown()); // ī��Ʈ�ٿ� ����
    }


    IEnumerator BatteryCountdown()
    {
        while (timer > 0)
        {
            // ���͸� �̹����� fillAmount�� ����
            Battery.fillAmount = timer / 60f;
            yield return new WaitForSeconds(0.1f); // 1�� ���
            timer--; // Ÿ�̸� ����

        }
        if (timer == 0)
        {
          
        }


    }

 
}
