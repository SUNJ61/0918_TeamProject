using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunstate : MonoBehaviour, IItem
{
    private Inventory inventory;
    private ItemData GunData;
    private GameObject Player;
    private Transform Camera_Tr;

    private float prevTime;

    private readonly float FireDist = 100.0f;
    private readonly float Delay = 0.2f;
    private readonly float Damage = 10.0f;
    private readonly float fireOffset = 0.5f;

    [SerializeField]private int initBullet = 1;
    public int InitBullet
    {
        get { return initBullet; }
        set
        {
            initBullet -= value;
            UpdateState();
        }
    }
    void Awake()
    {        
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
        Camera_Tr = Player.transform.GetChild(0).GetComponent<Transform>();

        GunData = new ItemData(gameObject, null, 2, "GunImg_Group");
    }
    void OnEnable()
    {
        UpdateState();
        prevTime = Time.time;
    }

    public void CatchItem()
    {
            inventory.GetItem(GunData);
    }

    public void Use()
    {
        if (InitBullet > 0 && Time.time - prevTime > Delay)
        {
            Ray ray = new Ray(Camera_Tr.position + (Camera_Tr.forward * fireOffset), Camera_Tr.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, FireDist, 1 << 7))
            {
                Debug.Log("발사");
                object[] param = new object[2];
                param[0] = hit.point;
                param[1] = Damage;
                if (hit.collider.CompareTag("BookHead"))
                {
                    Debug.Log("맞았다");
                    hit.collider.transform.SendMessage("OnDamage", param, SendMessageOptions.DontRequireReceiver);
                }
            }
            AddBullet(-1);
            prevTime = Time.time;
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.OnPlayerUI_Text("공기총 [E]");
        else
            InGameUIManager.instance.OnPlayerUI_Text("인벤토리가 꽉 찼습니다.");
    }

    private void UpdateState()
    {
        if (InitBullet <= 1)
            inventory.CanReload = true;
        else
            inventory.CanReload = false;
    }
    public void AddBullet(int value)
    {
        InitBullet = -value;
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(GunData.Item_Icon, GunData.ItemUIGroup);
    }

    private void OnDisable() //슬롯이 꺼지면 업데이트
    {
        inventory.CanReload = false;
    }
}
