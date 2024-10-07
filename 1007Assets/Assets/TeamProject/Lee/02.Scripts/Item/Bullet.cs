using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int addBullet = 1; //아이템을 먹었을 때 장전되는 탄알 개수
    public int AddBullet
    {
        get { return addBullet; }
    }
}
