using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>();
    private Transform Camera_Tr;

    private float prevTime;

    private readonly float FireDist = 100.0f;
    private readonly float Delay = 0.2f;
    private readonly float Damage = 10.0f;
    private readonly float fireOffset = 0.5f;

    [SerializeField] private int inventory_Idx;
    public int Inventory_Idx
    {
        get { return inventory_Idx; }
        set { inventory_Idx = value; }
    }

    [SerializeField] private bool canShoot; //손에 아이템을 들었을 때 업데이트, 아이템을 든 상태에서 사용시 false로 업데이트
    public bool CanShoot
    {
        get { return canShoot; }
        set { canShoot = value; }
    }

    [SerializeField]private bool canHeal;
    public bool CanHeal
    {
        get { return canHeal; }
        set { canHeal = value; }
    }

    [SerializeField] private bool isFlash;
    public bool IsFlash
    {
        get { return isFlash; }
        set { isFlash = value; }
    }

    [SerializeField] private bool isUse; //마우스 왼쪽 버튼이 눌렸을 때 업데이트
    public bool IsUse
    {
        get { return isUse; }
        set { isUse = value; }
    }
    void Awake()
    {
        Camera_Tr = transform.GetChild(0).transform;

        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);

        prevTime = Time.time;
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        UsingItem();
    }
    private void UsingItem() //모든 아이템 사용 함수들 만들기.
    {
        UseFire();
        UseFlashLight();
        UseHealPack();
    }
    private void UseFire()
    {
        if (CanShoot && IsUse && Time.time - prevTime > Delay)
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
            if (ItemSlots[Inventory_Idx].transform.childCount != 0)
            {
                string item_name = ItemSlots[Inventory_Idx].transform.GetChild(0).name;
                if (item_name == "Gun")
                {
                    Gunstate gun = ItemSlots[Inventory_Idx].transform.GetChild(0).GetComponent<Gunstate>();
                    gun.InitBullet = 1;
                    prevTime = Time.time;
                }
            }
        }
    }

    private void UseFlashLight()
    {
        if (IsUse && IsFlash && Time.time - prevTime > Delay)
        {
                if (ItemSlots[Inventory_Idx].transform.childCount != 0)
                {
                    string item_name = ItemSlots[Inventory_Idx].transform.GetChild(0).name;
                    if (item_name == "flashlight")
                    {
                        FlashLight flash = ItemSlots[Inventory_Idx].transform.GetChild(0).GetComponent<FlashLight>();
                        flash.SendMessage("ToggleFlashlights", SendMessageOptions.DontRequireReceiver);
                        prevTime = Time.time;
                    }
                }
        }
    }

    private void UseHealPack()
    {
        if(IsUse && CanHeal && Time.time - prevTime > Delay)
        {
            if (ItemSlots[Inventory_Idx].transform.childCount != 0)
            {
                string item_name = ItemSlots[Inventory_Idx].transform.GetChild(0).name;
                if (item_name == "HealPack")
                {
                    HealPack healpack = ItemSlots[Inventory_Idx].transform.GetChild(0).GetComponent<HealPack>();
                    healpack.SendMessage("HealPlayer", SendMessageOptions.DontRequireReceiver);
                    prevTime = Time.time;
                }
            }
        }
    }
}
