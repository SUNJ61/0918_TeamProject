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

    private readonly float Delay = 0.2f;
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
        inventory.GetItem(HealPackData);
    }

    public void Use()
    {
        if(playerHealth.health < 10 && Time.time - prevTime > Delay)
        {
            playerHealth.AddHealth(Heal_Amount);
            InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon,  HealPackData.ItemUIGroup);
            Destroy(this.gameObject);
            inventory.CanGetItem = true;
            prevTime = Time.time;
        }
        else if(playerHealth.health >= 10)
        {
            InGameUIManager.instance.OnPlayerUI_Text("ü���� ���� �ʾҽ��ϴ�.");
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.OnPlayerUI_Text("���޻��� [E]");            
        else
            InGameUIManager.instance.OnPlayerUI_Text("�κ��丮�� �� á���ϴ�.");
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon, HealPackData.ItemUIGroup);
    }
}
