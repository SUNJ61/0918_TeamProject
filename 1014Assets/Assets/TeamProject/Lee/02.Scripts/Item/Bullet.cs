using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;

    private readonly int AddBullet = 1; //�������� �Ծ��� �� �����Ǵ� ź�� ����

    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
    }

    public void CatchItem() //�÷��̾ ����ִ� gun�� �Ѿ˼��� 1�� ���� ��Ų��.
    {
        if (inventory.GetGun && inventory.CanReload)
        {
            inventory.AddBullet(AddBullet);
            Destroy(this.gameObject);
        }
    }

    public void Use() 
    {
        //�κ��丮���� ����ϴ� ������ �ƴ� ���� x.
    }

    public void ItemUIOn() //�ش� �������� UI�� ����.
    {
        if(inventory.GetGun && inventory.CanReload)
            InGameUIManager.instance.OnPlayerUI_Text("�Ѿ� ���� [E]");
        else
            InGameUIManager.instance.OnPlayerUI_Text("������ ������ �����ϴ�.");
    }
}
