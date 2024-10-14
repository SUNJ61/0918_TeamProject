using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;

    private readonly int AddBullet = 1; //아이템을 먹었을 때 장전되는 탄약 개수

    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
    }

    public void CatchItem() //플레이어가 들고있는 gun에 총알수를 1개 증가 시킨다.
    {
        if (inventory.GetGun && inventory.CanReload)
        {
            inventory.AddBullet(AddBullet);
            Destroy(this.gameObject);
        }
    }

    public void Use() 
    {
        //인벤토리에서 사용하는 아이템 아님 구현 x.
    }

    public void ItemUIOn() //해당 아이템의 UI를 띄운다.
    {
        if(inventory.GetGun && inventory.CanReload)
            InGameUIManager.instance.OnPlayerUI_Text("총알 장전 [E]");
        else
            InGameUIManager.instance.OnPlayerUI_Text("장전할 공간이 없습니다.");
    }
}
