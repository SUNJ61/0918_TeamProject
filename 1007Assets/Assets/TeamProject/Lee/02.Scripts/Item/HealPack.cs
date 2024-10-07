using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    private string ParentName;

    private readonly float Heal_Amount = 3.0f;
    private void Awake()
    {
        if (transform.parent != null)
            ParentName = transform.parent.parent.parent.name;
    }

    void Update()
    {
        if (transform.parent != null)
            ParentName = transform.parent.parent.parent.name; // 건 위치 정보 계속 업데이트

        if (ParentName == "Player") //아이템 슬롯 안에 있을 때
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();

            item.CanHeal = true;
        }
    }

    private void HealPlayer()
    {
        PlayerHealth playerHealth = transform.parent.parent.parent.GetComponent<PlayerHealth>();
        playerHealth.AddHealth(Heal_Amount);
        if (ParentName == "Player")
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();
            item.CanHeal = false;
        }
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        if (ParentName == "Player")
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();
            item.CanHeal = false;
        }
    }
}
