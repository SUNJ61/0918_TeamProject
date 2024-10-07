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
            ParentName = transform.parent.parent.parent.name; // 건 위치 정보 계속 업데이트

        if (ParentName == "Player") //아이템 슬롯 안에 있을 때
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
        else //총을 먹고 버렸을 때를 총알 먹는걸 막아야함.
        {
            CamerRay bullet = GameObject.Find("Player").transform.GetChild(0).GetComponent<CamerRay>();
            bullet.CanReload = false;
        }
    }
    public void AddBullet(int value)
    {
        InitBullet = -value;
    }
    private void OnDisable() //슬롯이 꺼지면 업데이트
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
