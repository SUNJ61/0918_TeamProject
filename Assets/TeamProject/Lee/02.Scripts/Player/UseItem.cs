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

    [SerializeField] private bool canShoot; //�տ� ���� ����� �� ������Ʈ, ���� �� ���¿��� �Ѿ��� �پ��� false�� ������Ʈ
    public bool CanShoot
    {
        get { return canShoot; }
        set { canShoot = value; }
    }

    [SerializeField] private bool isFlash;
    public bool IsFlash
    {
        get { return isFlash; }
        set { isFlash = value; }
    }

    [SerializeField] private bool isUse; //���콺 ���� ��ư�� ������ �� ������Ʈ
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
        UpdateItemSlots();
    }
    private void UsingItem() //��� ������ ��� �Լ��� �����.
    {
        UseFire();
        UseFlashLight();
    }
    private void UseFire()
    {
        if (CanShoot && IsUse && Time.time - prevTime > Delay)
        {
            Ray ray = new Ray(Camera_Tr.position + (Camera_Tr.forward * fireOffset), Camera_Tr.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, FireDist, 1 << 7))
            {
                object[] param = new object[2];
                param[0] = hit.point;
                param[1] = Damage;
                if (hit.collider.CompareTag("BookHead"))
                {
                    hit.collider.transform.SendMessage("OnDamage", param, SendMessageOptions.DontRequireReceiver);
                }
            }
            for (int i = 0; i < ItemSlots.Count - 1; i++)
            {
                if (ItemSlots[i].transform.childCount != 0)
                {
                    string item_name = ItemSlots[i].transform.GetChild(0).name;
                    if (item_name == "Gun")
                    {
                        Gunstate gun = ItemSlots[i].transform.GetChild(0).GetComponent<Gunstate>();
                        gun.InitBullet = 1;
                        prevTime = Time.time;
                        break;
                    }
                }
            }
        }
    }

    private void UseFlashLight()
    {
        if (IsUse && IsFlash && Time.time - prevTime > Delay)
        {
            for (int i = 0; i < ItemSlots.Count - 1; i++)
            {
                if (ItemSlots[i].transform.childCount != 0)
                {
                    string item_name = ItemSlots[i].transform.GetChild(0).name;
                    if (item_name == "flashlight")
                    {
                        FlashLight flash = ItemSlots[i].transform.GetChild(0).GetComponent<FlashLight>();
                        flash.SendMessage("ToggleFlashlights", SendMessageOptions.DontRequireReceiver);
                        prevTime = Time.time;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateItemSlots()
    {

    }
}
