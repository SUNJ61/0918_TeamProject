using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : CretureUpdate
{
    private object[] param = new object[2];

    private int MobCount;
    [SerializeField]private int flash_Index = 0;
    public int Flash_Index
    {
        get { return flash_Index; }
        set { flash_Index = value +1; }
    }

    private readonly float Enemy_Damage = 5.0f;

    private readonly string Hitbox_DTag = "Hitbox_D";
    private readonly string Hitbox_HTag = "Hitbox_H";

    private void Awake()
    {
        param[1] = Enemy_Damage;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDamage(object[] param)
    {
        base.OnDamage(param);
        if (dead && MobCount == 0)
        {
            GameManager.G_instance.isGameover = true;

            GameObject demon = (GameObject)param[0];
            demon.SendMessage("KillPlayer");
        }
        else if(dead && MobCount == 1)
        {
            GameManager.G_instance.isGameover = true;

            GameObject bookhead = (GameObject)param[0];
            bookhead.SendMessage("KillPlayer");
        }
    }

    public override void AddHealth(float AddHealth)
    {
        base.AddHealth(AddHealth);
    }

    public override void Die()
    {
        base.Die();
        this.gameObject.SetActive(false);
    }

    private void ShowEffect() //맞았을 때 이펙트구현 함수
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Hitbox_DTag))
        {
            MobCount = 0;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
            ShowEffect(); //블러드 이펙트
        }
        else if (other.gameObject.CompareTag(Hitbox_HTag))
        {
            MobCount = 1;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
        }
    }
}
