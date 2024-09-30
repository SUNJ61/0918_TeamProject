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
        ParentName = transform.parent.parent.parent.name;
    }
    void Update()
    {
        ParentName = transform.parent.parent.parent.name; // �� ��ġ ���� ��� ������Ʈ

        if (ParentName == "Player") //������ ���� �ȿ� ���� ��
        {
            UseItem item = transform.parent.parent.parent.GetComponent<UseItem>();

            if (InitBullet != 0)
                item.CanShoot = true;
            else if (InitBullet == 0)
                item.CanShoot = false;
        }
    }
}
