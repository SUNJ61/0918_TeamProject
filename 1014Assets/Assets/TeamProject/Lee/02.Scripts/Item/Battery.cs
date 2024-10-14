using UnityEngine;

public class Battery : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
    }

    public void CatchItem()
    {
        if (inventory.CanGetBattery && inventory.GetFlash)
        {
            inventory.AddBattery();
            Destroy(gameObject);
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetBattery && inventory.GetFlash)
            InGameUIManager.instance.OnPlayerUI_Text("배터리 교체 [E]");
        else
        {
            InGameUIManager.instance.OnPlayerUI_Text("배터리 교체가 불가능합니다.");
        }
    }

    public void Use()
    {
        //인벤토리에서 사용하는 아이템 아님 구현 x.
    }
}
