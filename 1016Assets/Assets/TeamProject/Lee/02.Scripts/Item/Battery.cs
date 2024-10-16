using UnityEngine;

public class Battery : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;

    private float prevTime;

    private readonly float CatchDelay = 0.05f;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
    }

    void OnEnable()
    {
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (inventory.CanGetBattery && inventory.GetFlash && Time.time - prevTime > CatchDelay)
        {
            prevTime = Time.time;
            inventory.AddBattery();           
            Destroy(gameObject);         
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetBattery && inventory.GetFlash)
            InGameUIManager.instance.SetPlayerUI_Text("배터리 교체 [E]");
        else
        {
            InGameUIManager.instance.SetPlayerUI_Text("배터리 교체가 불가능합니다.");
        }
    }

    public void Use()
    {
        //인벤토리에서 사용하는 아이템 아님 구현 x.
    }
}
