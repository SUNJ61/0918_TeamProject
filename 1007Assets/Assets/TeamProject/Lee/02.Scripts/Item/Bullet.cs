using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int addBullet = 1; //�������� �Ծ��� �� �����Ǵ� ź�� ����
    public int AddBullet
    {
        get { return addBullet; }
    }
}
