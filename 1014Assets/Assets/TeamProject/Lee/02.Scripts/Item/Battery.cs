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
            InGameUIManager.instance.OnPlayerUI_Text("���͸� ��ü [E]");
        else
        {
            InGameUIManager.instance.OnPlayerUI_Text("���͸� ��ü�� �Ұ����մϴ�.");
        }
    }

    public void Use()
    {
        //�κ��丮���� ����ϴ� ������ �ƴ� ���� x.
    }
}
