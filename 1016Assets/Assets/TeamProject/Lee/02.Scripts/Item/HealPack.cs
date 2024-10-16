using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour, IItem
{
    private ItemData HealPackData;

    private GameObject Player;
    private Inventory inventory;
    private PlayerHealth playerHealth;

    private float prevTime;

    private readonly float CatchDelay = 0.05f;
    private readonly float UseDelay = 0.2f;
    private readonly float Heal_Amount = 3.0f;

    private void Awake()
    {
        HealPackData = new ItemData(gameObject, null, 3, "HealPackImg_Group");
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
        playerHealth = Player.GetComponent<PlayerHealth>();
    }
    private void OnEnable()
    { 
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (Time.time - prevTime > CatchDelay) //������ ȣ�� �Ǵ� �� ����.
        {
            Debug.Log("ȣǮ");
            prevTime = Time.time;
            inventory.GetItem(HealPackData);           
        }
    }

    public void Use()
    {
        if(playerHealth.health < 10 && Time.time - prevTime > UseDelay)
        {
            prevTime = Time.time;
            playerHealth.AddHealth(Heal_Amount);
            InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon,  HealPackData.ItemUIGroup);
            Destroy(this.gameObject);
            inventory.CanGetItem = true;            
        }
        else if(playerHealth.health >= 10)
        {
            InGameUIManager.instance.SetPlayerUI_Text("ü���� ���� �ʾҽ��ϴ�.");
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.SetPlayerUI_Text("���޻��� [E]");            
        else
            InGameUIManager.instance.SetPlayerUI_Text("�κ��丮�� �� á���ϴ�.");
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon, HealPackData.ItemUIGroup);
    }
}
