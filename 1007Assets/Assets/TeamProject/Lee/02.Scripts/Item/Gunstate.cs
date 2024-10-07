using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunstate : MonoBehaviour
{
    [SerializeField]private string ParentName;

    private int initBullet = 1;
    public int InitBullet
    {
        get { return initBullet; }
        set { initBullet -= value; }
    }
    void Awake()
    {
        if(transform.parent != null)
            ParentName = transform.parent.parent.parent.name;
    }
    void Update()
    {
        if(transform.parent != null)
            ParentName = transform.parent.parent.parent.name; // �� ��ġ ���� ��� ������Ʈ

        if (ParentName == "Player") //������ ���� �ȿ� ���� ��
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();
            CamerRay bullet = transform.parent.parent.GetComponent<CamerRay>();

            if (InitBullet == 0)
            {
                item.CanShoot = false;
                bullet.CanReload = true;
            }
            else if (InitBullet <= 1)
            {
                item.CanShoot = true;
                bullet.CanReload = true;
            }
            else
            {
                item.CanShoot = true;
                bullet.CanReload = false;
            }
        }
        else //���� �԰� ������ ���� �Ѿ� �Դ°� ���ƾ���.
        {
            CamerRay bullet = GameObject.Find("Player").transform.GetChild(0).GetComponent<CamerRay>();
            bullet.CanReload = false;
        }
    }
    public void AddBullet(int value)
    {
        InitBullet = -value;
    }
    private void OnDisable() //������ ������ ������Ʈ
    {
        if (ParentName == "Player")
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();
            CamerRay bullet = transform.parent.parent.GetComponent<CamerRay>();

            item.CanShoot = false;
            bullet.CanReload = false;
        }
    }
}
