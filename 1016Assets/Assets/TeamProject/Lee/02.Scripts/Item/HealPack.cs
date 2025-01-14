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
        if (Time.time - prevTime > CatchDelay) //여러번 호출 되는 것 방지.
        {
            Debug.Log("호풀");
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
            InGameUIManager.instance.SetPlayerUI_Text("체력이 닳지 않았습니다.");
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.SetPlayerUI_Text("구급상자 [E]");            
        else
            InGameUIManager.instance.SetPlayerUI_Text("인벤토리가 꽉 찼습니다.");
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon, HealPackData.ItemUIGroup);
    }
}
