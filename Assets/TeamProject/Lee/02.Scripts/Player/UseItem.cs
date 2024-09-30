using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>();
    private Transform CamTransform;

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
    [SerializeField] private bool isFire; //���콺 ���� ��ư�� ������ �� ������Ʈ
    public bool IsFire
    {
        get { return isFire; }
        set { isFire = value; }
    }
    void Awake()
    {
        CamTransform = transform.GetChild(0).transform;

        for (int i = 0; i < CamTransform.childCount; i++)
        {
            ItemSlots.Add(CamTransform.GetChild(i).gameObject);
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
        Fire();
    }
    private void Fire()
    {
        if (CanShoot && IsFire && Time.time - prevTime > Delay)
        {
            Ray ray = new Ray(CamTransform.position + (CamTransform.forward * fireOffset), CamTransform.forward);
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
                if (ItemSlots[i].transform.GetChild(0) != null)
                {
                    string item_name = ItemSlots[i].transform.GetChild(0).name;
                    if (item_name == "Gun")
                    {
                        Gunstate gun = ItemSlots[i].transform.GetChild(0).GetComponent<Gunstate>();
                        gun.InitBullet = 1;
                        break;
                    }
                }
            }
            prevTime = Time.time;
        }
    }

    private void UpdateItemSlots()
    {

    }
}
